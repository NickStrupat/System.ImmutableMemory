using System.Buffers;
using System.ComponentModel;

namespace System
{
	public readonly struct ImmutableMemory<T>
	{
		internal readonly ReadOnlyMemory<T> memory;
		public ImmutableMemory(ReadOnlySpan<T> source) => memory = source.IsEmpty ? ReadOnlyMemory<T>.Empty : source.ToArray();
		internal ImmutableMemory(ReadOnlyMemory<T> memory) => this.memory = memory;

		public ReadOnlyMemory<T> AsMemory() => memory;
		public ReadOnlySpan<T> AsSpan() => memory.Span;
		public ImmutableSpan<T> AsImmutableSpan() => new ImmutableSpan<T>(this);

		public static ImmutableMemory<T> Empty => default;

		public Int32 Length => memory.Length;
		public Boolean IsEmpty => memory.IsEmpty;

		public void CopyTo(Memory<T> destination) => memory.CopyTo(destination);
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Boolean Equals(Object obj) => obj is ImmutableMemory<T> im && memory.Equals(im);
		public Boolean Equals(ImmutableMemory<T> other) => memory.Equals(other.memory);
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Int32 GetHashCode() => memory.GetHashCode();
		public MemoryHandle Pin() => memory.Pin();
		public ImmutableMemory<T> Slice(Int32 start, Int32 length) => new ImmutableMemory<T>(memory.Slice(start, length));
		public ImmutableMemory<T> Slice(Int32 start) => new ImmutableMemory<T>(memory.Slice(start));
		public T[] ToArray() => memory.ToArray();
		public override String ToString() => memory.ToString();
		public Boolean TryCopyTo(Memory<T> destination) => memory.TryCopyTo(destination);

		public static ImmutableMemory<T> Create<TState>(Int32 length, TState state, SpanAction<T, TState> action)
		{
			var array = new T[length];
			action(array.AsSpan(), state);
			return new ImmutableMemory<T>(array.AsMemory());
		}
	}

#if !NETSTANDARD_2_1
	public delegate void SpanAction<T, in TArg>(Span<T> span, TArg arg);
#endif
}
