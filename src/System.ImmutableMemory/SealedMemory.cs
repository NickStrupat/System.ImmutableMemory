#if NET
using System.Buffers;
using System.Runtime.InteropServices;
using System.Sealing;
using System.Threading;

namespace System
{
	/// <summary>
	/// Owns a copy of some elements in memory that the hardware itself treats as read-only. Any write, including
	/// through <c>MemoryMarshal</c>, <c>Unsafe</c> or a pinned pointer, raises an access violation that ends the
	/// process.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Prototype. Small buffers are packed into shared 64 KB chunks. The chunk currently being filled still has a
	/// writable alias inside this library until it is full; larger buffers get their own pages and never have one after
	/// creation. On Linux, code in the same process can still <c>mprotect</c> the pages writable; on macOS and Windows
	/// it can't.
	/// </para>
	/// <para>
	/// Dispose it once nothing reads it any more. Spans and pointers taken from <see cref="Memory"/> must not outlive
	/// the <see cref="SealedMemory{T}"/>: after disposal the pages may be unmapped. Without disposal the memory is
	/// never freed.
	/// </para>
	/// </remarks>
	/// <typeparam name="T">The type of the elements.</typeparam>
	public sealed class SealedMemory<T> : IDisposable where T : unmanaged
	{
		private readonly Manager manager;
		private readonly SealedArena.Chunk chunk;
		private Int32 disposed;

		private SealedMemory(Manager manager, SealedArena.Chunk chunk)
		{
			this.manager = manager;
			this.chunk = chunk;
		}

		/// <summary>Gets whether this platform supports sealed memory (macOS, Linux and Windows).</summary>
		public static Boolean IsSupported => DoubleMapping.IsSupported;

		/// <summary>Copies <paramref name="source"/> into newly sealed memory.</summary>
		/// <param name="source">The elements to copy.</param>
		/// <exception cref="PlatformNotSupportedException">The platform has no way to double-map memory.</exception>
		public static unsafe SealedMemory<T> Create(ReadOnlySpan<T> source)
		{
			if (source.IsEmpty)
				return new SealedMemory<T>(null, null);
			var allocation = SealedArena.Allocate(MemoryMarshal.AsBytes(source));
			return new SealedMemory<T>(new Manager((T*)allocation.Address, source.Length), allocation.Chunk);
		}

		/// <summary>Gets the sealed elements.</summary>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		public ImmutableMemory<T> Memory
		{
			get
			{
				ObjectDisposedException.ThrowIf(Volatile.Read(ref disposed) != 0, this);
				return manager is null ? ImmutableMemory<T>.Empty : new ImmutableMemory<T>(manager.Memory);
			}
		}

		/// <summary>
		/// Releases the memory. Afterwards <see cref="Memory"/> and any <see cref="ImmutableMemory{T}"/> taken from it
		/// throw <see cref="ObjectDisposedException"/> when accessed.
		/// </summary>
		public void Dispose()
		{
			if (Interlocked.Exchange(ref disposed, 1) != 0 || manager is null)
				return;
			manager.MarkDisposed();
			SealedArena.Release(chunk);
		}

		private sealed unsafe class Manager : MemoryManager<T>
		{
			private readonly T* pointer;
			private readonly Int32 length;
			private Int32 disposed;

			public Manager(T* pointer, Int32 length)
			{
				this.pointer = pointer;
				this.length = length;
			}

			public void MarkDisposed() => Volatile.Write(ref disposed, 1);

			public override Span<T> GetSpan()
			{
				ObjectDisposedException.ThrowIf(Volatile.Read(ref disposed) != 0, this);
				return new Span<T>(pointer, length);
			}

			public override MemoryHandle Pin(Int32 elementIndex = 0)
			{
				ObjectDisposedException.ThrowIf(Volatile.Read(ref disposed) != 0, this);
				ArgumentOutOfRangeException.ThrowIfGreaterThan((UInt32)elementIndex, (UInt32)length, nameof(elementIndex));
				return new MemoryHandle(pointer + elementIndex);
			}

			public override void Unpin()
			{
			}

			protected override void Dispose(Boolean disposing)
			{
			}
		}
	}
}
#endif
