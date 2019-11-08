using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace System
{
	public readonly ref struct ImmutableSpan<T>
	{
		internal readonly ReadOnlySpan<T> span;
		public ImmutableSpan(ImmutableMemory<T> immutableMemory) => span = immutableMemory.memory.Span;
		internal ImmutableSpan(ReadOnlySpan<T> span) => this.span = span;
		public static ImmutableSpan<T> Empty => default;

		public ReadOnlySpan<T> AsSpan() => span;

		public ref readonly T this[Int32 index] => ref span[index];

		public Int32 Length => span.Length;
		public Boolean IsEmpty => span.IsEmpty;

		public void CopyTo(Span<T> destination) => span.CopyTo(destination);

		public Enumerator GetEnumerator() => new Enumerator(this);

#pragma warning disable CS0809
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Equals() on ReadOnlySpan will always throw an exception. Use == instead.")]
		public override Boolean Equals(Object obj) => throw new NotSupportedException();

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("GetHashCode() on ReadOnlySpan will always throw an exception.")]
		public override Int32 GetHashCode() => throw new NotSupportedException();
#pragma warning restore CS0809

		[EditorBrowsable(EditorBrowsableState.Never)]
		public ref readonly T GetPinnableReference() => ref span.GetPinnableReference();
		public ImmutableSpan<T> Slice(Int32 start) => new ImmutableSpan<T>(span.Slice(start));
		public ImmutableSpan<T> Slice(Int32 start, Int32 length) => new ImmutableSpan<T>(span.Slice(start, length));
		
		public T[] ToArray() => span.ToArray();
		public override String ToString() => span.ToString();
		public Boolean TryCopyTo(Span<T> destination) => span.TryCopyTo(destination);
		
		public static Boolean operator ==(ImmutableSpan<T> left, ImmutableSpan<T> right) => left.span == right.span;
		public static Boolean operator !=(ImmutableSpan<T> left, ImmutableSpan<T> right) => left.span == right.span;

		public ref struct Enumerator
		{
			private readonly ImmutableSpan<T> span;
			private int index;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal Enumerator(ImmutableSpan<T> span)
			{
				this.span = span;
				index = -1;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool MoveNext()
			{
				int index = this.index + 1;
				if (index < span.Length)
				{
					this.index = index;
					return true;
				}

				return false;
			}

			public ref readonly T Current {
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get => ref span[index];
			}
		}
	}
}
