#if NET
namespace System.Sealing
{
	// Packs small sealed buffers into shared chunks so each one doesn't cost a whole page. Only the chunk currently
	// being filled has a writable view; a chunk is retired (writable view unmapped) as soon as the next buffer doesn't
	// fit, and its read-only view is unmapped once it is retired and every buffer in it has been released.
	internal static class SealedArena
	{
		internal const Int32 ChunkSize = 64 * 1024; // also Windows' allocation granularity
		private const Int32 LargeThreshold = ChunkSize / 2;
		private const Int32 Alignment = 16;

		private static readonly Object gate = new Object();
		private static Chunk current;

		public static unsafe Allocation Allocate(ReadOnlySpan<Byte> source)
		{
			lock (gate)
			{
				if (source.Length > LargeThreshold)
				{
					var own = new Chunk(RoundUp(source.Length, Environment.SystemPageSize));
					var large = own.Add(source, 0);
					own.Retire();
					return large;
				}

				var offset = current is null ? 0 : RoundUp(current.Used, Alignment);
				if (current is null || offset + source.Length > ChunkSize)
				{
					current?.Retire();
					current = new Chunk(ChunkSize);
					offset = 0;
				}
				return current.Add(source, offset);
			}
		}

		public static void Release(Chunk chunk)
		{
			lock (gate)
				chunk.Release();
		}

		private static Int32 RoundUp(Int32 value, Int32 multiple) => (value + multiple - 1) / multiple * multiple;

		internal readonly struct Allocation
		{
			public readonly Chunk Chunk;
			public readonly IntPtr Address;

			public Allocation(Chunk chunk, IntPtr address)
			{
				Chunk = chunk;
				Address = address;
			}
		}

		internal sealed class Chunk
		{
			private readonly DoubleMapping mapping;
			private Int32 live;
			private Boolean retired;

			public Chunk(Int32 size) => mapping = DoubleMapping.Create((nuint)size);

			public Int32 Used { get; private set; }

			public unsafe Allocation Add(ReadOnlySpan<Byte> source, Int32 offset)
			{
				source.CopyTo(new Span<Byte>((Byte*)mapping.Writable + offset, source.Length));
				Used = offset + source.Length;
				live++;
				return new Allocation(this, mapping.ReadOnly + offset);
			}

			public void Retire()
			{
				mapping.UnmapWritable();
				retired = true;
				if (live == 0)
					mapping.UnmapReadOnly();
			}

			public void Release()
			{
				live--;
				if (retired && live == 0)
					mapping.UnmapReadOnly();
			}
		}
	}
}
#endif
