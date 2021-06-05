using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;

namespace System
{
	public static class ImmutableMemoryExtensions
	{
		public static ImmutableMemory<T> ToImmutableMemory<T>(this ReadOnlySpan<T> span) => new ImmutableMemory<T>(span);
		public static ImmutableMemory<T> ToImmutableMemory<T>(this ReadOnlySpan<T> span, Int32 start) => new ImmutableMemory<T>(span.Slice(start));
		public static ImmutableMemory<T> ToImmutableMemory<T>(this ReadOnlySpan<T> span, Int32 start, Int32 length) => new ImmutableMemory<T>(span.Slice(start, length));

		public static ImmutableMemory<T> ToImmutableMemory<T>(this Span<T> span) => new ImmutableMemory<T>(span);
		public static ImmutableMemory<T> ToImmutableMemory<T>(this Span<T> span, Int32 start) => new ImmutableMemory<T>(span.Slice(start));
		public static ImmutableMemory<T> ToImmutableMemory<T>(this Span<T> span, Int32 start, Int32 length) => new ImmutableMemory<T>(span.Slice(start, length));

		public static ImmutableMemory<T> ToImmutableMemory<T>(this ReadOnlyMemory<T> memory) => new ImmutableMemory<T>(memory.Span);
		public static ImmutableMemory<T> ToImmutableMemory<T>(this ReadOnlyMemory<T> span, Int32 start) => new ImmutableMemory<T>(span.Span.Slice(start));
		public static ImmutableMemory<T> ToImmutableMemory<T>(this ReadOnlyMemory<T> span, Int32 start, Int32 length) => new ImmutableMemory<T>(span.Span.Slice(start, length));

		public static ImmutableMemory<T> ToImmutableMemory<T>(this Memory<T> memory) => new ImmutableMemory<T>(memory.Span);
		public static ImmutableMemory<T> ToImmutableMemory<T>(this Memory<T> span, Int32 start) => new ImmutableMemory<T>(span.Span.Slice(start));
		public static ImmutableMemory<T> ToImmutableMemory<T>(this Memory<T> span, Int32 start, Int32 length) => new ImmutableMemory<T>(span.Span.Slice(start, length));

		public static ImmutableMemory<T> ToImmutableMemory<T>(this T[] array) => new ImmutableMemory<T>(array.AsSpan());
		public static ImmutableMemory<T> ToImmutableMemory<T>(this T[] span, Int32 start) => new ImmutableMemory<T>(span.AsSpan(start));
		public static ImmutableMemory<T> ToImmutableMemory<T>(this T[] span, Int32 start, Int32 length) => new ImmutableMemory<T>(span.AsSpan(start, length));

		public static ImmutableMemory<T> ToImmutableMemory<T>(this ArraySegment<T> array) => new ImmutableMemory<T>(array.AsSpan());
		public static ImmutableMemory<T> ToImmutableMemory<T>(this ArraySegment<T> span, Int32 start) => new ImmutableMemory<T>(span.AsSpan(start));
		public static ImmutableMemory<T> ToImmutableMemory<T>(this ArraySegment<T> span, Int32 start, Int32 length) => new ImmutableMemory<T>(span.AsSpan(start, length));

		public static ImmutableMemory<Char> AsImmutableMemory(this String @string) => new ImmutableMemory<Char>(@string.AsMemory());
		public static ImmutableMemory<Char> AsImmutableMemory(this String @string, Int32 start) => new ImmutableMemory<Char>(@string.AsMemory(start));
		public static ImmutableMemory<Char> AsImmutableMemory(this String @string, Int32 start, Int32 length) => new ImmutableMemory<Char>(@string.AsMemory(start, length));

		public static ImmutableSpan<Char> AsImmutableSpan(this String text) => text.AsImmutableMemory().AsImmutableSpan();
		public static ImmutableSpan<Char> AsImmutableSpan(this String text, Int32 start) => text.AsImmutableMemory(start).AsImmutableSpan();
		public static ImmutableSpan<Char> AsImmutableSpan(this String text, Int32 start, Int32 length) => text.AsImmutableMemory(start, length).AsImmutableSpan();

		public static ImmutableMemory<T> AsImmutableMemory<T>(this ImmutableArray<T> array) => new ImmutableMemory<T>(array.AsMemory());
		public static ImmutableMemory<T> AsImmutableMemory<T>(this ImmutableArray<T> array, Int32 start) => new ImmutableMemory<T>(array.AsMemory().Slice(start));
		public static ImmutableMemory<T> AsImmutableMemory<T>(this ImmutableArray<T> array, Int32 start, Int32 length) => new ImmutableMemory<T>(array.AsMemory().Slice(start, length));
		
		public static ImmutableSpan<T> AsImmutableSpan<T>(this ImmutableArray<T> array) => new ImmutableSpan<T>(array.AsSpan());
		public static ImmutableSpan<T> AsImmutableSpan<T>(this ImmutableArray<T> array, Int32 start) => new ImmutableSpan<T>(array.AsSpan().Slice(start));
		public static ImmutableSpan<T> AsImmutableSpan<T>(this ImmutableArray<T> array, Int32 start, Int32 length) => new ImmutableSpan<T>(array.AsSpan().Slice(start, length));

		public static Int32 BinarySearch<T, TComparer>(this ImmutableSpan<T> span, T value, TComparer comparer) where TComparer : IComparer<T> => span.AsSpan().BinarySearch(value, comparer);
		public static Int32 BinarySearch<T, TComparable>(this ImmutableSpan<T> span, TComparable comparable) where TComparable : IComparable<T> => span.AsSpan().BinarySearch(comparable);
		public static Int32 BinarySearch<T>(this ImmutableSpan<T> span, IComparable<T> comparable) => span.AsSpan().BinarySearch(comparable);

		public static Int32 CompareTo(this ImmutableSpan<Char> span, ReadOnlySpan<Char> other, StringComparison comparisonType) => span.AsSpan().CompareTo(other, comparisonType);

		public static Boolean Contains(this ImmutableSpan<Char> span, ReadOnlySpan<Char> value, StringComparison comparisonType) => span.AsSpan().Contains(value, comparisonType);

		public static Boolean EndsWith<T>(this ImmutableSpan<T> span, ReadOnlySpan<T> value) where T : IEquatable<T> => span.AsSpan().EndsWith(value);
		public static Boolean EndsWith(this ImmutableSpan<Char> span, ReadOnlySpan<Char> value, StringComparison comparisonType) => span.AsSpan().EndsWith(value, comparisonType);

		public static Boolean Equals(this ImmutableSpan<Char> span, ReadOnlySpan<Char> other, StringComparison comparisonType) => span.AsSpan().Equals(other, comparisonType);

		public static Int32 IndexOf<T>(this ImmutableSpan<T> span, ReadOnlySpan<T> value) where T : IEquatable<T> => span.AsSpan().IndexOf(value);
		public static Int32 IndexOf<T>(this ImmutableSpan<T> span, T value) where T : IEquatable<T> => span.AsSpan().IndexOf(value);
		public static Int32 IndexOf(this ImmutableSpan<Char> span, ReadOnlySpan<Char> value, StringComparison comparisonType) => span.AsSpan().IndexOf(value, comparisonType);

		public static Int32 IndexOfAny<T>(this ImmutableSpan<T> span, ReadOnlySpan<T> values) where T : IEquatable<T> => span.AsSpan().IndexOfAny(values);
		public static Int32 IndexOfAny<T>(this ImmutableSpan<T> span, T value0, T value1) where T : IEquatable<T> => span.AsSpan().IndexOfAny(value0, value1);
		public static Int32 IndexOfAny<T>(this ImmutableSpan<T> span, T value0, T value1, T value2) where T : IEquatable<T> => span.AsSpan().IndexOfAny(value0, value1, value2);

		public static Boolean IsWhiteSpace(this ImmutableSpan<Char> span) => span.AsSpan().IsWhiteSpace();

		public static Int32 LastIndexOf<T>(this ImmutableSpan<T> span, T value) where T : IEquatable<T> => span.AsSpan().LastIndexOf(value);
		public static Int32 LastIndexOf<T>(this ImmutableSpan<T> span, ReadOnlySpan<T> value) where T : IEquatable<T> => span.AsSpan().LastIndexOf(value);

		public static Int32 LastIndexOfAny<T>(this ImmutableSpan<T> span, ReadOnlySpan<T> values) where T : IEquatable<T> => span.AsSpan().LastIndexOfAny(values);
		public static Int32 LastIndexOfAny<T>(this ImmutableSpan<T> span, T value0, T value1) where T : IEquatable<T> => span.AsSpan().LastIndexOfAny(value0, value1);
		public static Int32 LastIndexOfAny<T>(this ImmutableSpan<T> span, T value0, T value1, T value2) where T : IEquatable<T> => span.AsSpan().LastIndexOfAny(value0, value1, value2);

		public static Boolean Overlaps<T>(this ImmutableSpan<T> span, ReadOnlySpan<T> other) => span.AsSpan().Overlaps(other);
		public static Boolean Overlaps<T>(this ImmutableSpan<T> span, ReadOnlySpan<T> other, out Int32 elementOffset) => span.AsSpan().Overlaps(other, out elementOffset);

		public static Int32 SequenceCompareTo<T>(this ImmutableSpan<T> span, ReadOnlySpan<T> other) where T : IComparable<T> => span.AsSpan().SequenceCompareTo(other);

		public static Boolean SequenceEqual<T>(this ImmutableSpan<T> span, ReadOnlySpan<T> other) where T : IEquatable<T> => span.AsSpan().SequenceEqual(other);

		public static Boolean StartsWith<T>(this ImmutableSpan<T> span, ReadOnlySpan<T> value) where T : IEquatable<T> => span.AsSpan().StartsWith(value);
		public static Boolean StartsWith(this ImmutableSpan<Char> span, ReadOnlySpan<Char> value, StringComparison comparisonType) => span.AsSpan().StartsWith(value, comparisonType);

		public static Int32 ToLower(this ImmutableSpan<Char> source, Span<Char> destination, CultureInfo culture) => source.AsSpan().ToLower(destination, culture);
		public static Int32 ToLowerInvariant(this ImmutableSpan<Char> source, Span<Char> destination) => source.AsSpan().ToLowerInvariant(destination);
		public static Int32 ToUpper(this ImmutableSpan<Char> source, Span<Char> destination, CultureInfo culture) => source.AsSpan().ToUpper(destination, culture);
		public static Int32 ToUpperInvariant(this ImmutableSpan<Char> source, Span<Char> destination) => source.AsSpan().ToUpperInvariant(destination);

		public static ImmutableSpan<Char> Trim(this ImmutableSpan<Char> span, Char trimChar) => new ImmutableSpan<Char>(span.AsSpan().Trim(trimChar));
		public static ImmutableSpan<Char> Trim(this ImmutableSpan<Char> span) => new ImmutableSpan<Char>(span.AsSpan().Trim());
		public static ImmutableSpan<Char> Trim(this ImmutableSpan<Char> span, ReadOnlySpan<Char> trimChars) => new ImmutableSpan<Char>(span.AsSpan().Trim(trimChars));

		public static ImmutableSpan<Char> TrimEnd(this ImmutableSpan<Char> span, ReadOnlySpan<Char> trimChars) => new ImmutableSpan<Char>(span.AsSpan().TrimEnd(trimChars));
		public static ImmutableSpan<Char> TrimEnd(this ImmutableSpan<Char> span) => new ImmutableSpan<Char>(span.AsSpan().TrimEnd());
		public static ImmutableSpan<Char> TrimEnd(this ImmutableSpan<Char> span, Char trimChar) => new ImmutableSpan<Char>(span.AsSpan().TrimEnd(trimChar));

		public static ImmutableSpan<Char> TrimStart(this ImmutableSpan<Char> span, Char trimChar) => new ImmutableSpan<Char>(span.AsSpan().TrimStart(trimChar));
		public static ImmutableSpan<Char> TrimStart(this ImmutableSpan<Char> span) => new ImmutableSpan<Char>(span.AsSpan().TrimStart());
		public static ImmutableSpan<Char> TrimStart(this ImmutableSpan<Char> span, ReadOnlySpan<Char> trimChars) => new ImmutableSpan<Char>(span.AsSpan().TrimStart(trimChars));
	}
}
