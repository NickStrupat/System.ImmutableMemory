using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;

namespace System
{
	/// <summary>
	/// Conversions to <see cref="ImmutableMemory{T}"/> and <see cref="ImmutableSpan{T}"/>, and the
	/// non-mutating <see cref="MemoryExtensions"/> methods for <see cref="ImmutableSpan{T}"/>.
	/// </summary>
	public static class ImmutableMemoryExtensions
	{
		/// <summary>Copies the elements of <paramref name="span"/> into a new <see cref="ImmutableMemory{T}"/>.</summary>
		/// <param name="span">The elements to copy.</param>
		public static ImmutableMemory<T> ToImmutableMemory<T>(this ReadOnlySpan<T> span) => new ImmutableMemory<T>(span);
		/// <summary>Copies the elements of <paramref name="span"/> from <paramref name="start"/> to the end, into a new <see cref="ImmutableMemory{T}"/>.</summary>
		/// <param name="span">The elements to copy.</param>
		/// <param name="start">The index to start at.</param>
		/// <exception cref="ArgumentOutOfRangeException">The range is outside the source.</exception>
		public static ImmutableMemory<T> ToImmutableMemory<T>(this ReadOnlySpan<T> span, Int32 start) => new ImmutableMemory<T>(span.Slice(start));
		/// <summary>Copies the elements of <paramref name="span"/> starting at <paramref name="start"/>, <paramref name="length"/> elements long, into a new <see cref="ImmutableMemory{T}"/>.</summary>
		/// <param name="span">The elements to copy.</param>
		/// <param name="start">The index to start at.</param>
		/// <param name="length">The number of elements.</param>
		/// <exception cref="ArgumentOutOfRangeException">The range is outside the source.</exception>
		public static ImmutableMemory<T> ToImmutableMemory<T>(this ReadOnlySpan<T> span, Int32 start, Int32 length) => new ImmutableMemory<T>(span.Slice(start, length));

		/// <summary>Copies the elements of <paramref name="span"/> into a new <see cref="ImmutableMemory{T}"/>.</summary>
		/// <param name="span">The elements to copy.</param>
		public static ImmutableMemory<T> ToImmutableMemory<T>(this Span<T> span) => new ImmutableMemory<T>(span);
		/// <summary>Copies the elements of <paramref name="span"/> from <paramref name="start"/> to the end, into a new <see cref="ImmutableMemory{T}"/>.</summary>
		/// <param name="span">The elements to copy.</param>
		/// <param name="start">The index to start at.</param>
		/// <exception cref="ArgumentOutOfRangeException">The range is outside the source.</exception>
		public static ImmutableMemory<T> ToImmutableMemory<T>(this Span<T> span, Int32 start) => new ImmutableMemory<T>(span.Slice(start));
		/// <summary>Copies the elements of <paramref name="span"/> starting at <paramref name="start"/>, <paramref name="length"/> elements long, into a new <see cref="ImmutableMemory{T}"/>.</summary>
		/// <param name="span">The elements to copy.</param>
		/// <param name="start">The index to start at.</param>
		/// <param name="length">The number of elements.</param>
		/// <exception cref="ArgumentOutOfRangeException">The range is outside the source.</exception>
		public static ImmutableMemory<T> ToImmutableMemory<T>(this Span<T> span, Int32 start, Int32 length) => new ImmutableMemory<T>(span.Slice(start, length));

		/// <summary>Copies the elements of <paramref name="memory"/> into a new <see cref="ImmutableMemory{T}"/>.</summary>
		/// <param name="memory">The elements to copy.</param>
		public static ImmutableMemory<T> ToImmutableMemory<T>(this ReadOnlyMemory<T> memory) => new ImmutableMemory<T>(memory.Span);
		/// <summary>Copies the elements of <paramref name="memory"/> from <paramref name="start"/> to the end, into a new <see cref="ImmutableMemory{T}"/>.</summary>
		/// <param name="memory">The elements to copy.</param>
		/// <param name="start">The index to start at.</param>
		/// <exception cref="ArgumentOutOfRangeException">The range is outside the source.</exception>
		public static ImmutableMemory<T> ToImmutableMemory<T>(this ReadOnlyMemory<T> memory, Int32 start) => new ImmutableMemory<T>(memory.Span.Slice(start));
		/// <summary>Copies the elements of <paramref name="memory"/> starting at <paramref name="start"/>, <paramref name="length"/> elements long, into a new <see cref="ImmutableMemory{T}"/>.</summary>
		/// <param name="memory">The elements to copy.</param>
		/// <param name="start">The index to start at.</param>
		/// <param name="length">The number of elements.</param>
		/// <exception cref="ArgumentOutOfRangeException">The range is outside the source.</exception>
		public static ImmutableMemory<T> ToImmutableMemory<T>(this ReadOnlyMemory<T> memory, Int32 start, Int32 length) => new ImmutableMemory<T>(memory.Span.Slice(start, length));

		/// <summary>Copies the elements of <paramref name="memory"/> into a new <see cref="ImmutableMemory{T}"/>.</summary>
		/// <param name="memory">The elements to copy.</param>
		public static ImmutableMemory<T> ToImmutableMemory<T>(this Memory<T> memory) => new ImmutableMemory<T>(memory.Span);
		/// <summary>Copies the elements of <paramref name="memory"/> from <paramref name="start"/> to the end, into a new <see cref="ImmutableMemory{T}"/>.</summary>
		/// <param name="memory">The elements to copy.</param>
		/// <param name="start">The index to start at.</param>
		/// <exception cref="ArgumentOutOfRangeException">The range is outside the source.</exception>
		public static ImmutableMemory<T> ToImmutableMemory<T>(this Memory<T> memory, Int32 start) => new ImmutableMemory<T>(memory.Span.Slice(start));
		/// <summary>Copies the elements of <paramref name="memory"/> starting at <paramref name="start"/>, <paramref name="length"/> elements long, into a new <see cref="ImmutableMemory{T}"/>.</summary>
		/// <param name="memory">The elements to copy.</param>
		/// <param name="start">The index to start at.</param>
		/// <param name="length">The number of elements.</param>
		/// <exception cref="ArgumentOutOfRangeException">The range is outside the source.</exception>
		public static ImmutableMemory<T> ToImmutableMemory<T>(this Memory<T> memory, Int32 start, Int32 length) => new ImmutableMemory<T>(memory.Span.Slice(start, length));

		/// <summary>Copies the elements of <paramref name="array"/> into a new <see cref="ImmutableMemory{T}"/>.</summary>
		/// <param name="array">The elements to copy.</param>
		public static ImmutableMemory<T> ToImmutableMemory<T>(this T[] array) => new ImmutableMemory<T>(array.AsSpan());
		/// <summary>Copies the elements of <paramref name="array"/> from <paramref name="start"/> to the end, into a new <see cref="ImmutableMemory{T}"/>.</summary>
		/// <param name="array">The elements to copy.</param>
		/// <param name="start">The index to start at.</param>
		/// <exception cref="ArgumentOutOfRangeException">The range is outside the source.</exception>
		public static ImmutableMemory<T> ToImmutableMemory<T>(this T[] array, Int32 start) => new ImmutableMemory<T>(array.AsSpan(start));
		/// <summary>Copies the elements of <paramref name="array"/> starting at <paramref name="start"/>, <paramref name="length"/> elements long, into a new <see cref="ImmutableMemory{T}"/>.</summary>
		/// <param name="array">The elements to copy.</param>
		/// <param name="start">The index to start at.</param>
		/// <param name="length">The number of elements.</param>
		/// <exception cref="ArgumentOutOfRangeException">The range is outside the source.</exception>
		public static ImmutableMemory<T> ToImmutableMemory<T>(this T[] array, Int32 start, Int32 length) => new ImmutableMemory<T>(array.AsSpan(start, length));

		/// <summary>Copies the elements of <paramref name="segment"/> into a new <see cref="ImmutableMemory{T}"/>.</summary>
		/// <param name="segment">The elements to copy.</param>
		public static ImmutableMemory<T> ToImmutableMemory<T>(this ArraySegment<T> segment) => new ImmutableMemory<T>(segment.AsSpan());
		/// <summary>Copies the elements of <paramref name="segment"/> from <paramref name="start"/> to the end, into a new <see cref="ImmutableMemory{T}"/>.</summary>
		/// <param name="segment">The elements to copy.</param>
		/// <param name="start">The index to start at.</param>
		/// <exception cref="ArgumentOutOfRangeException">The range is outside the source.</exception>
		public static ImmutableMemory<T> ToImmutableMemory<T>(this ArraySegment<T> segment, Int32 start) => new ImmutableMemory<T>(segment.AsSpan(start));
		/// <summary>Copies the elements of <paramref name="segment"/> starting at <paramref name="start"/>, <paramref name="length"/> elements long, into a new <see cref="ImmutableMemory{T}"/>.</summary>
		/// <param name="segment">The elements to copy.</param>
		/// <param name="start">The index to start at.</param>
		/// <param name="length">The number of elements.</param>
		/// <exception cref="ArgumentOutOfRangeException">The range is outside the source.</exception>
		public static ImmutableMemory<T> ToImmutableMemory<T>(this ArraySegment<T> segment, Int32 start, Int32 length) => new ImmutableMemory<T>(segment.AsSpan(start, length));

		/// <summary>Wraps <paramref name="string"/> as an <see cref="ImmutableMemory{T}"/> of <see cref="Char"/>, without copying, since strings are immutable.</summary>
		/// <param name="string">The string to wrap.</param>
		public static ImmutableMemory<Char> AsImmutableMemory(this String @string) => new ImmutableMemory<Char>(@string.AsMemory());
		/// <summary>Wraps the part of <paramref name="string"/> from <paramref name="start"/> to the end as an <see cref="ImmutableMemory{T}"/> of <see cref="Char"/>, without copying, since strings are immutable.</summary>
		/// <param name="string">The string to wrap.</param>
		/// <param name="start">The index to start at.</param>
		/// <exception cref="ArgumentOutOfRangeException">The range is outside the source.</exception>
		public static ImmutableMemory<Char> AsImmutableMemory(this String @string, Int32 start) => new ImmutableMemory<Char>(@string.AsMemory(start));
		/// <summary>Wraps the part of <paramref name="string"/> starting at <paramref name="start"/>, <paramref name="length"/> elements long, as an <see cref="ImmutableMemory{T}"/> of <see cref="Char"/>, without copying, since strings are immutable.</summary>
		/// <param name="string">The string to wrap.</param>
		/// <param name="start">The index to start at.</param>
		/// <param name="length">The number of elements.</param>
		/// <exception cref="ArgumentOutOfRangeException">The range is outside the source.</exception>
		public static ImmutableMemory<Char> AsImmutableMemory(this String @string, Int32 start, Int32 length) => new ImmutableMemory<Char>(@string.AsMemory(start, length));

		/// <summary>Wraps <paramref name="text"/> as an <see cref="ImmutableSpan{T}"/> of <see cref="Char"/>, without copying, since strings are immutable.</summary>
		/// <param name="text">The string to wrap.</param>
		public static ImmutableSpan<Char> AsImmutableSpan(this String text) => text.AsImmutableMemory().AsImmutableSpan();
		/// <summary>Wraps the part of <paramref name="text"/> from <paramref name="start"/> to the end as an <see cref="ImmutableSpan{T}"/> of <see cref="Char"/>, without copying, since strings are immutable.</summary>
		/// <param name="text">The string to wrap.</param>
		/// <param name="start">The index to start at.</param>
		/// <exception cref="ArgumentOutOfRangeException">The range is outside the source.</exception>
		public static ImmutableSpan<Char> AsImmutableSpan(this String text, Int32 start) => text.AsImmutableMemory(start).AsImmutableSpan();
		/// <summary>Wraps the part of <paramref name="text"/> starting at <paramref name="start"/>, <paramref name="length"/> elements long, as an <see cref="ImmutableSpan{T}"/> of <see cref="Char"/>, without copying, since strings are immutable.</summary>
		/// <param name="text">The string to wrap.</param>
		/// <param name="start">The index to start at.</param>
		/// <param name="length">The number of elements.</param>
		/// <exception cref="ArgumentOutOfRangeException">The range is outside the source.</exception>
		public static ImmutableSpan<Char> AsImmutableSpan(this String text, Int32 start, Int32 length) => text.AsImmutableMemory(start, length).AsImmutableSpan();

		/// <summary>Wraps <paramref name="array"/> as an <see cref="ImmutableMemory{T}"/>, without copying, since <see cref="ImmutableArray{T}"/> contents are immutable.</summary>
		/// <param name="array">The array to wrap.</param>
		public static ImmutableMemory<T> AsImmutableMemory<T>(this ImmutableArray<T> array) => new ImmutableMemory<T>(array.AsMemory());
		/// <summary>Wraps the part of <paramref name="array"/> from <paramref name="start"/> to the end as an <see cref="ImmutableMemory{T}"/>, without copying, since <see cref="ImmutableArray{T}"/> contents are immutable.</summary>
		/// <param name="array">The array to wrap.</param>
		/// <param name="start">The index to start at.</param>
		/// <exception cref="ArgumentOutOfRangeException">The range is outside the source.</exception>
		public static ImmutableMemory<T> AsImmutableMemory<T>(this ImmutableArray<T> array, Int32 start) => new ImmutableMemory<T>(array.AsMemory().Slice(start));
		/// <summary>Wraps the part of <paramref name="array"/> starting at <paramref name="start"/>, <paramref name="length"/> elements long, as an <see cref="ImmutableMemory{T}"/>, without copying, since <see cref="ImmutableArray{T}"/> contents are immutable.</summary>
		/// <param name="array">The array to wrap.</param>
		/// <param name="start">The index to start at.</param>
		/// <param name="length">The number of elements.</param>
		/// <exception cref="ArgumentOutOfRangeException">The range is outside the source.</exception>
		public static ImmutableMemory<T> AsImmutableMemory<T>(this ImmutableArray<T> array, Int32 start, Int32 length) => new ImmutableMemory<T>(array.AsMemory().Slice(start, length));
		
		/// <summary>Wraps <paramref name="array"/> as an <see cref="ImmutableSpan{T}"/>, without copying, since <see cref="ImmutableArray{T}"/> contents are immutable.</summary>
		/// <param name="array">The array to wrap.</param>
		public static ImmutableSpan<T> AsImmutableSpan<T>(this ImmutableArray<T> array) => new ImmutableSpan<T>(array.AsSpan());
		/// <summary>Wraps the part of <paramref name="array"/> from <paramref name="start"/> to the end as an <see cref="ImmutableSpan{T}"/>, without copying, since <see cref="ImmutableArray{T}"/> contents are immutable.</summary>
		/// <param name="array">The array to wrap.</param>
		/// <param name="start">The index to start at.</param>
		/// <exception cref="ArgumentOutOfRangeException">The range is outside the source.</exception>
		public static ImmutableSpan<T> AsImmutableSpan<T>(this ImmutableArray<T> array, Int32 start) => new ImmutableSpan<T>(array.AsSpan().Slice(start));
		/// <summary>Wraps the part of <paramref name="array"/> starting at <paramref name="start"/>, <paramref name="length"/> elements long, as an <see cref="ImmutableSpan{T}"/>, without copying, since <see cref="ImmutableArray{T}"/> contents are immutable.</summary>
		/// <param name="array">The array to wrap.</param>
		/// <param name="start">The index to start at.</param>
		/// <param name="length">The number of elements.</param>
		/// <exception cref="ArgumentOutOfRangeException">The range is outside the source.</exception>
		public static ImmutableSpan<T> AsImmutableSpan<T>(this ImmutableArray<T> array, Int32 start, Int32 length) => new ImmutableSpan<T>(array.AsSpan().Slice(start, length));

		/// <inheritdoc cref="MemoryExtensions.BinarySearch{T, TComparer}(ReadOnlySpan{T}, T, TComparer)"/>
		public static Int32 BinarySearch<T, TComparer>(this ImmutableSpan<T> span, T value, TComparer comparer) where TComparer : IComparer<T> => span.AsSpan().BinarySearch(value, comparer);
		/// <inheritdoc cref="MemoryExtensions.BinarySearch{T, TComparable}(ReadOnlySpan{T}, TComparable)"/>
		public static Int32 BinarySearch<T, TComparable>(this ImmutableSpan<T> span, TComparable comparable) where TComparable : IComparable<T> => span.AsSpan().BinarySearch(comparable);
		/// <inheritdoc cref="MemoryExtensions.BinarySearch{T}(ReadOnlySpan{T}, IComparable{T})"/>
		public static Int32 BinarySearch<T>(this ImmutableSpan<T> span, IComparable<T> comparable) => span.AsSpan().BinarySearch(comparable);

		/// <inheritdoc cref="MemoryExtensions.CompareTo(ReadOnlySpan{Char}, ReadOnlySpan{Char}, StringComparison)"/>
		public static Int32 CompareTo(this ImmutableSpan<Char> span, ReadOnlySpan<Char> other, StringComparison comparisonType) => span.AsSpan().CompareTo(other, comparisonType);

		/// <inheritdoc cref="MemoryExtensions.Contains(ReadOnlySpan{Char}, ReadOnlySpan{Char}, StringComparison)"/>
		public static Boolean Contains(this ImmutableSpan<Char> span, ReadOnlySpan<Char> value, StringComparison comparisonType) => span.AsSpan().Contains(value, comparisonType);

		/// <inheritdoc cref="MemoryExtensions.EndsWith{T}(ReadOnlySpan{T}, ReadOnlySpan{T})"/>
		public static Boolean EndsWith<T>(this ImmutableSpan<T> span, ReadOnlySpan<T> value) where T : IEquatable<T> => span.AsSpan().EndsWith(value);
		/// <inheritdoc cref="MemoryExtensions.EndsWith(ReadOnlySpan{Char}, ReadOnlySpan{Char}, StringComparison)"/>
		public static Boolean EndsWith(this ImmutableSpan<Char> span, ReadOnlySpan<Char> value, StringComparison comparisonType) => span.AsSpan().EndsWith(value, comparisonType);

		/// <inheritdoc cref="MemoryExtensions.Equals(ReadOnlySpan{Char}, ReadOnlySpan{Char}, StringComparison)"/>
		public static Boolean Equals(this ImmutableSpan<Char> span, ReadOnlySpan<Char> other, StringComparison comparisonType) => span.AsSpan().Equals(other, comparisonType);

		/// <inheritdoc cref="MemoryExtensions.IndexOf{T}(ReadOnlySpan{T}, ReadOnlySpan{T})"/>
		public static Int32 IndexOf<T>(this ImmutableSpan<T> span, ReadOnlySpan<T> value) where T : IEquatable<T> => span.AsSpan().IndexOf(value);
		/// <inheritdoc cref="MemoryExtensions.IndexOf{T}(ReadOnlySpan{T}, T)"/>
		public static Int32 IndexOf<T>(this ImmutableSpan<T> span, T value) where T : IEquatable<T> => span.AsSpan().IndexOf(value);
		/// <inheritdoc cref="MemoryExtensions.IndexOf(ReadOnlySpan{Char}, ReadOnlySpan{Char}, StringComparison)"/>
		public static Int32 IndexOf(this ImmutableSpan<Char> span, ReadOnlySpan<Char> value, StringComparison comparisonType) => span.AsSpan().IndexOf(value, comparisonType);

		/// <inheritdoc cref="MemoryExtensions.IndexOfAny{T}(ReadOnlySpan{T}, ReadOnlySpan{T})"/>
		public static Int32 IndexOfAny<T>(this ImmutableSpan<T> span, ReadOnlySpan<T> values) where T : IEquatable<T> => span.AsSpan().IndexOfAny(values);
		/// <inheritdoc cref="MemoryExtensions.IndexOfAny{T}(ReadOnlySpan{T}, T, T)"/>
		public static Int32 IndexOfAny<T>(this ImmutableSpan<T> span, T value0, T value1) where T : IEquatable<T> => span.AsSpan().IndexOfAny(value0, value1);
		/// <inheritdoc cref="MemoryExtensions.IndexOfAny{T}(ReadOnlySpan{T}, T, T, T)"/>
		public static Int32 IndexOfAny<T>(this ImmutableSpan<T> span, T value0, T value1, T value2) where T : IEquatable<T> => span.AsSpan().IndexOfAny(value0, value1, value2);

		/// <inheritdoc cref="MemoryExtensions.IsWhiteSpace(ReadOnlySpan{Char})"/>
		public static Boolean IsWhiteSpace(this ImmutableSpan<Char> span) => span.AsSpan().IsWhiteSpace();

		/// <inheritdoc cref="MemoryExtensions.LastIndexOf{T}(ReadOnlySpan{T}, T)"/>
		public static Int32 LastIndexOf<T>(this ImmutableSpan<T> span, T value) where T : IEquatable<T> => span.AsSpan().LastIndexOf(value);
		/// <inheritdoc cref="MemoryExtensions.LastIndexOf{T}(ReadOnlySpan{T}, ReadOnlySpan{T})"/>
		public static Int32 LastIndexOf<T>(this ImmutableSpan<T> span, ReadOnlySpan<T> value) where T : IEquatable<T> => span.AsSpan().LastIndexOf(value);

		/// <inheritdoc cref="MemoryExtensions.LastIndexOfAny{T}(ReadOnlySpan{T}, ReadOnlySpan{T})"/>
		public static Int32 LastIndexOfAny<T>(this ImmutableSpan<T> span, ReadOnlySpan<T> values) where T : IEquatable<T> => span.AsSpan().LastIndexOfAny(values);
		/// <inheritdoc cref="MemoryExtensions.LastIndexOfAny{T}(ReadOnlySpan{T}, T, T)"/>
		public static Int32 LastIndexOfAny<T>(this ImmutableSpan<T> span, T value0, T value1) where T : IEquatable<T> => span.AsSpan().LastIndexOfAny(value0, value1);
		/// <inheritdoc cref="MemoryExtensions.LastIndexOfAny{T}(ReadOnlySpan{T}, T, T, T)"/>
		public static Int32 LastIndexOfAny<T>(this ImmutableSpan<T> span, T value0, T value1, T value2) where T : IEquatable<T> => span.AsSpan().LastIndexOfAny(value0, value1, value2);

		/// <inheritdoc cref="MemoryExtensions.Overlaps{T}(ReadOnlySpan{T}, ReadOnlySpan{T})"/>
		public static Boolean Overlaps<T>(this ImmutableSpan<T> span, ReadOnlySpan<T> other) => span.AsSpan().Overlaps(other);
		/// <inheritdoc cref="MemoryExtensions.Overlaps{T}(ReadOnlySpan{T}, ReadOnlySpan{T}, out Int32)"/>
		public static Boolean Overlaps<T>(this ImmutableSpan<T> span, ReadOnlySpan<T> other, out Int32 elementOffset) => span.AsSpan().Overlaps(other, out elementOffset);

		/// <inheritdoc cref="MemoryExtensions.SequenceCompareTo{T}(ReadOnlySpan{T}, ReadOnlySpan{T})"/>
		public static Int32 SequenceCompareTo<T>(this ImmutableSpan<T> span, ReadOnlySpan<T> other) where T : IComparable<T> => span.AsSpan().SequenceCompareTo(other);

		/// <inheritdoc cref="MemoryExtensions.SequenceEqual{T}(ReadOnlySpan{T}, ReadOnlySpan{T})"/>
		public static Boolean SequenceEqual<T>(this ImmutableSpan<T> span, ReadOnlySpan<T> other) where T : IEquatable<T> => span.AsSpan().SequenceEqual(other);

		/// <inheritdoc cref="MemoryExtensions.StartsWith{T}(ReadOnlySpan{T}, ReadOnlySpan{T})"/>
		public static Boolean StartsWith<T>(this ImmutableSpan<T> span, ReadOnlySpan<T> value) where T : IEquatable<T> => span.AsSpan().StartsWith(value);
		/// <inheritdoc cref="MemoryExtensions.StartsWith(ReadOnlySpan{Char}, ReadOnlySpan{Char}, StringComparison)"/>
		public static Boolean StartsWith(this ImmutableSpan<Char> span, ReadOnlySpan<Char> value, StringComparison comparisonType) => span.AsSpan().StartsWith(value, comparisonType);

		/// <inheritdoc cref="MemoryExtensions.ToLower(ReadOnlySpan{Char}, Span{Char}, CultureInfo)"/>
		public static Int32 ToLower(this ImmutableSpan<Char> source, Span<Char> destination, CultureInfo culture) => source.AsSpan().ToLower(destination, culture);
		/// <inheritdoc cref="MemoryExtensions.ToLowerInvariant(ReadOnlySpan{Char}, Span{Char})"/>
		public static Int32 ToLowerInvariant(this ImmutableSpan<Char> source, Span<Char> destination) => source.AsSpan().ToLowerInvariant(destination);
		/// <inheritdoc cref="MemoryExtensions.ToUpper(ReadOnlySpan{Char}, Span{Char}, CultureInfo)"/>
		public static Int32 ToUpper(this ImmutableSpan<Char> source, Span<Char> destination, CultureInfo culture) => source.AsSpan().ToUpper(destination, culture);
		/// <inheritdoc cref="MemoryExtensions.ToUpperInvariant(ReadOnlySpan{Char}, Span{Char})"/>
		public static Int32 ToUpperInvariant(this ImmutableSpan<Char> source, Span<Char> destination) => source.AsSpan().ToUpperInvariant(destination);

		/// <inheritdoc cref="MemoryExtensions.Trim(ReadOnlySpan{Char}, Char)"/>
		public static ImmutableSpan<Char> Trim(this ImmutableSpan<Char> span, Char trimChar) => new ImmutableSpan<Char>(span.AsSpan().Trim(trimChar));
		/// <inheritdoc cref="MemoryExtensions.Trim(ReadOnlySpan{Char})"/>
		public static ImmutableSpan<Char> Trim(this ImmutableSpan<Char> span) => new ImmutableSpan<Char>(span.AsSpan().Trim());
		/// <inheritdoc cref="MemoryExtensions.Trim(ReadOnlySpan{Char}, ReadOnlySpan{Char})"/>
		public static ImmutableSpan<Char> Trim(this ImmutableSpan<Char> span, ReadOnlySpan<Char> trimChars) => new ImmutableSpan<Char>(span.AsSpan().Trim(trimChars));

		/// <inheritdoc cref="MemoryExtensions.TrimEnd(ReadOnlySpan{Char}, ReadOnlySpan{Char})"/>
		public static ImmutableSpan<Char> TrimEnd(this ImmutableSpan<Char> span, ReadOnlySpan<Char> trimChars) => new ImmutableSpan<Char>(span.AsSpan().TrimEnd(trimChars));
		/// <inheritdoc cref="MemoryExtensions.TrimEnd(ReadOnlySpan{Char})"/>
		public static ImmutableSpan<Char> TrimEnd(this ImmutableSpan<Char> span) => new ImmutableSpan<Char>(span.AsSpan().TrimEnd());
		/// <inheritdoc cref="MemoryExtensions.TrimEnd(ReadOnlySpan{Char}, Char)"/>
		public static ImmutableSpan<Char> TrimEnd(this ImmutableSpan<Char> span, Char trimChar) => new ImmutableSpan<Char>(span.AsSpan().TrimEnd(trimChar));

		/// <inheritdoc cref="MemoryExtensions.TrimStart(ReadOnlySpan{Char}, Char)"/>
		public static ImmutableSpan<Char> TrimStart(this ImmutableSpan<Char> span, Char trimChar) => new ImmutableSpan<Char>(span.AsSpan().TrimStart(trimChar));
		/// <inheritdoc cref="MemoryExtensions.TrimStart(ReadOnlySpan{Char})"/>
		public static ImmutableSpan<Char> TrimStart(this ImmutableSpan<Char> span) => new ImmutableSpan<Char>(span.AsSpan().TrimStart());
		/// <inheritdoc cref="MemoryExtensions.TrimStart(ReadOnlySpan{Char}, ReadOnlySpan{Char})"/>
		public static ImmutableSpan<Char> TrimStart(this ImmutableSpan<Char> span, ReadOnlySpan<Char> trimChars) => new ImmutableSpan<Char>(span.AsSpan().TrimStart(trimChars));
	}
}
