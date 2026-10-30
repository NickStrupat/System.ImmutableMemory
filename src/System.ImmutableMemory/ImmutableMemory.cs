using System.Buffers;
using System.ComponentModel;

namespace System
{
	/// <summary>
	/// A contiguous region of memory whose contents are guaranteed not to change, unlike <see cref="ReadOnlyMemory{T}"/>,
	/// which only prevents mutation through itself.
	/// </summary>
	/// <remarks>
	/// The memory either wraps an object known to be immutable (<see cref="String"/>,
	/// <see cref="System.Collections.Immutable.ImmutableArray{T}"/>) or owns a defensive copy that nothing else can reach.
	/// The guarantee does not hold against code that uses <c>MemoryMarshal</c>, pointers from <see cref="Pin"/>, or
	/// <see cref="ImmutableSpan{T}.UnsafeCreateWithoutCopy"/>.
	/// </remarks>
	/// <typeparam name="T">The type of the elements.</typeparam>
	public readonly struct ImmutableMemory<T> : IEquatable<ImmutableMemory<T>>
	{
		internal readonly ReadOnlyMemory<T> memory;

		/// <summary>Creates an <see cref="ImmutableMemory{T}"/> holding a copy of <paramref name="source"/>.</summary>
		/// <param name="source">The elements to copy.</param>
		public ImmutableMemory(ReadOnlySpan<T> source) => memory = source.IsEmpty ? ReadOnlyMemory<T>.Empty : source.ToArray();
		internal ImmutableMemory(ReadOnlyMemory<T> memory) => this.memory = memory;

		/// <summary>Returns a read-only view of the memory, for APIs that take <see cref="ReadOnlyMemory{T}"/>.</summary>
		public ReadOnlyMemory<T> AsMemory() => memory;
		/// <summary>Returns a read-only span over the memory, for APIs that take <see cref="ReadOnlySpan{T}"/>.</summary>
		public ReadOnlySpan<T> AsSpan() => memory.Span;
		/// <summary>Returns an <see cref="ImmutableSpan{T}"/> over the memory.</summary>
		public ImmutableSpan<T> AsImmutableSpan() => new ImmutableSpan<T>(this);

		/// <summary>Gets an empty <see cref="ImmutableMemory{T}"/>.</summary>
		public static ImmutableMemory<T> Empty => default;

		/// <summary>Gets the number of elements.</summary>
		public Int32 Length => memory.Length;
		/// <summary>Gets whether the memory has no elements.</summary>
		public Boolean IsEmpty => memory.IsEmpty;

		/// <summary>Copies the elements into <paramref name="destination"/>.</summary>
		/// <param name="destination">The memory to copy into.</param>
		/// <exception cref="ArgumentException"><paramref name="destination"/> is shorter than this memory.</exception>
		public void CopyTo(Memory<T> destination) => memory.CopyTo(destination);
		/// <inheritdoc cref="Equals(ImmutableMemory{T})"/>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Boolean Equals(Object obj) => obj is ImmutableMemory<T> im && memory.Equals(im.memory);
		/// <summary>
		/// Returns whether both instances refer to the same region of the same underlying object. Contents are not
		/// compared, so two separate copies of equal elements are not equal.
		/// </summary>
		/// <param name="other">The instance to compare with.</param>
		public Boolean Equals(ImmutableMemory<T> other) => memory.Equals(other.memory);
		/// <inheritdoc/>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Int32 GetHashCode() => memory.GetHashCode();
		/// <summary>Pins the memory so its address stays fixed. Writing through the pointer breaks immutability.</summary>
		/// <returns>A handle that must be disposed to unpin the memory.</returns>
		public MemoryHandle Pin() => memory.Pin();
		/// <summary>Returns a slice of <paramref name="length"/> elements starting at <paramref name="start"/>, without copying.</summary>
		/// <param name="start">The index the slice starts at.</param>
		/// <param name="length">The number of elements in the slice.</param>
		/// <exception cref="ArgumentOutOfRangeException">The range is outside this memory.</exception>
		public ImmutableMemory<T> Slice(Int32 start, Int32 length) => new ImmutableMemory<T>(memory.Slice(start, length));
		/// <summary>Returns a slice from <paramref name="start"/> to the end, without copying.</summary>
		/// <param name="start">The index the slice starts at.</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="start"/> is outside this memory.</exception>
		public ImmutableMemory<T> Slice(Int32 start) => new ImmutableMemory<T>(memory.Slice(start));
		/// <summary>Copies the elements into a new array.</summary>
		public T[] ToArray() => memory.ToArray();
		/// <summary>
		/// For <see cref="Char"/>, returns the characters as a string; otherwise returns the type name and length.
		/// </summary>
		public override String ToString() => memory.ToString();
		/// <summary>Copies the elements into <paramref name="destination"/> if it is long enough.</summary>
		/// <param name="destination">The memory to copy into.</param>
		/// <returns><c>true</c> if the elements were copied; <c>false</c> if <paramref name="destination"/> is too short.</returns>
		public Boolean TryCopyTo(Memory<T> destination) => memory.TryCopyTo(destination);

		/// <summary>
		/// Creates an <see cref="ImmutableMemory{T}"/> of <paramref name="length"/> elements, filled by
		/// <paramref name="action"/> before anything else can see it.
		/// </summary>
		/// <param name="length">The number of elements.</param>
		/// <param name="state">A value passed to <paramref name="action"/>, so the delegate doesn't need to capture it.</param>
		/// <param name="action">Fills the span. It must not keep a reference to the span after returning.</param>
		/// <typeparam name="TState">The type of <paramref name="state"/>.</typeparam>
		/// <exception cref="OverflowException"><paramref name="length"/> is negative.</exception>
		/// <example>
		/// <code>
		/// var squares = ImmutableMemory&lt;Int32&gt;.Create(4, 0, (span, _) =>
		/// {
		///     for (var i = 0; i &lt; span.Length; i++)
		///         span[i] = i * i;
		/// });
		/// </code>
		/// </example>
#if NETSTANDARD2_0
		public static ImmutableMemory<T> Create<TState>(Int32 length, TState state, ImmutableMemoryInitializer<T, TState> action)
#else
		public static ImmutableMemory<T> Create<TState>(Int32 length, TState state, SpanAction<T, TState> action)
#endif
		{
			var array = new T[length];
			action(array.AsSpan(), state);
			return new ImmutableMemory<T>(array.AsMemory());
		}
	}
}
