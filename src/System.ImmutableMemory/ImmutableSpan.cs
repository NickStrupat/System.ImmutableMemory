using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace System
{
	/// <summary>
	/// A stack-only view of memory whose contents are guaranteed not to change, unlike <see cref="ReadOnlySpan{T}"/>,
	/// which only prevents mutation through itself.
	/// </summary>
	/// <remarks>
	/// Obtain one from <see cref="ImmutableMemory{T}"/>, a <see cref="String"/>, or an
	/// <see cref="System.Collections.Immutable.ImmutableArray{T}"/>. The same caveats as
	/// <see cref="ImmutableMemory{T}"/> apply.
	/// </remarks>
	/// <typeparam name="T">The type of the elements.</typeparam>
	public readonly ref struct ImmutableSpan<T>
	{
		private readonly ReadOnlySpan<T> span;
		/// <summary>Creates an <see cref="ImmutableSpan{T}"/> over <paramref name="immutableMemory"/>.</summary>
		/// <param name="immutableMemory">The memory to view.</param>
		public ImmutableSpan(ImmutableMemory<T> immutableMemory) => span = immutableMemory.memory.Span;
		internal ImmutableSpan(ReadOnlySpan<T> span) => this.span = span;
		/// <summary>Gets an empty <see cref="ImmutableSpan{T}"/>.</summary>
		public static ImmutableSpan<T> Empty => default;

		/// <summary>Returns the span as a <see cref="ReadOnlySpan{T}"/>, for APIs that take one.</summary>
		public ReadOnlySpan<T> AsSpan() => span;

		/// <summary>Gets a read-only reference to the element at <paramref name="index"/>.</summary>
		/// <param name="index">The index of the element.</param>
		/// <exception cref="IndexOutOfRangeException"><paramref name="index"/> is outside the span.</exception>
		public ref readonly T this[Int32 index] => ref span[index];

		/// <summary>Gets the number of elements.</summary>
		public Int32 Length => span.Length;
		/// <summary>Gets whether the span has no elements.</summary>
		public Boolean IsEmpty => span.IsEmpty;

		/// <summary>Copies the elements into <paramref name="destination"/>.</summary>
		/// <param name="destination">The span to copy into.</param>
		/// <exception cref="ArgumentException"><paramref name="destination"/> is shorter than this span.</exception>
		public void CopyTo(Span<T> destination) => span.CopyTo(destination);

		/// <summary>Returns an enumerator over the elements, for use with <c>foreach</c>.</summary>
		public Enumerator GetEnumerator() => new Enumerator(this);

#pragma warning disable CS0809
		/// <summary>Not supported. Use <c>==</c> instead.</summary>
		/// <param name="obj">Ignored.</param>
		/// <exception cref="NotSupportedException">Always thrown.</exception>
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Equals() on ImmutableSpan will always throw an exception. Use == instead.")]
		public override Boolean Equals(Object obj) => throw new NotSupportedException();

		/// <summary>Not supported.</summary>
		/// <exception cref="NotSupportedException">Always thrown.</exception>
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("GetHashCode() on ImmutableSpan will always throw an exception.")]
		public override Int32 GetHashCode() => throw new NotSupportedException();
#pragma warning restore CS0809

		/// <summary>
		/// Returns a reference to the first element, so the span can be used in a <c>fixed</c> statement. Writing through
		/// the pointer breaks immutability.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public ref readonly T GetPinnableReference() => ref span.GetPinnableReference();
		/// <summary>Returns a slice from <paramref name="start"/> to the end.</summary>
		/// <param name="start">The index the slice starts at.</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="start"/> is outside the span.</exception>
		public ImmutableSpan<T> Slice(Int32 start) => new ImmutableSpan<T>(span.Slice(start));
		/// <summary>Returns a slice of <paramref name="length"/> elements starting at <paramref name="start"/>.</summary>
		/// <param name="start">The index the slice starts at.</param>
		/// <param name="length">The number of elements in the slice.</param>
		/// <exception cref="ArgumentOutOfRangeException">The range is outside the span.</exception>
		public ImmutableSpan<T> Slice(Int32 start, Int32 length) => new ImmutableSpan<T>(span.Slice(start, length));

		/// <summary>Copies the elements into a new array.</summary>
		public T[] ToArray() => span.ToArray();
		/// <summary>
		/// For <see cref="Char"/>, returns the characters as a string; otherwise returns the type name and length.
		/// </summary>
		public override String ToString() => span.ToString();
		/// <summary>Copies the elements into <paramref name="destination"/> if it is long enough.</summary>
		/// <param name="destination">The span to copy into.</param>
		/// <returns><c>true</c> if the elements were copied; <c>false</c> if <paramref name="destination"/> is too short.</returns>
		public Boolean TryCopyTo(Span<T> destination) => span.TryCopyTo(destination);

		/// <summary>
		/// Returns whether both spans start at the same memory location and have the same length. Contents are not
		/// compared.
		/// </summary>
		/// <param name="left">The first span.</param>
		/// <param name="right">The second span.</param>
		public static Boolean operator ==(ImmutableSpan<T> left, ImmutableSpan<T> right) => left.span == right.span;
		/// <summary>Returns the opposite of <c>==</c>.</summary>
		/// <param name="left">The first span.</param>
		/// <param name="right">The second span.</param>
		public static Boolean operator !=(ImmutableSpan<T> left, ImmutableSpan<T> right) => left.span != right.span;

		/// <summary>Enumerates the elements of an <see cref="ImmutableSpan{T}"/>.</summary>
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

			/// <summary>Advances to the next element.</summary>
			/// <returns><c>true</c> if there is a next element; <c>false</c> at the end of the span.</returns>
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

			/// <summary>Gets a read-only reference to the current element.</summary>
			public ref readonly T Current {
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get => ref span[index];
			}
		}

		/// <summary>
		/// Wraps <paramref name="span"/> without making a defensive copy. Only use this when nothing else can write to
		/// the underlying memory for as long as the result is in use, e.g. a <c>stackalloc</c> buffer that is only
		/// reachable through <paramref name="span"/>.
		/// </summary>
		/// <param name="span">The span to wrap.</param>
		public static ImmutableSpan<T> UnsafeCreateWithoutCopy(ReadOnlySpan<T> span) => new(span);
	}
}
