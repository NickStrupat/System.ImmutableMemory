using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace System.ImmutableMemory
{
	public static class ImmutableMemoryExtensions
	{
		public static ImmutableMemory<Char> AsImmutableMemory(this String @string) => new ImmutableMemory<Char>(@string.AsMemory());
		public static ImmutableMemory<Char> AsImmutableMemory(this String @string, Int32 start) => new ImmutableMemory<Char>(@string.AsMemory(start));
		public static ImmutableMemory<Char> AsImmutableMemory(this String @string, Int32 start, Int32 length) => new ImmutableMemory<Char>(@string.AsMemory(start, length));
		public static ImmutableMemory<Byte> AsImmutableMemory(this String @string, Encoding encoding) => new ImmutableMemory<Byte>(encoding.GetBytes(@string).AsMemory());

		public static ReadOnlyMemory<T> AsMemory<T>(this ImmutableMemory<T> immutableMemory) => immutableMemory.memory;

		public static ImmutableSpan<Char> AsImmutableSpan(this String text) => text.AsImmutableMemory().AsImmutableSpan();
		public static ImmutableSpan<Char> AsImmutableSpan(this String text, Int32 start) => text.AsImmutableMemory(start).AsImmutableSpan();
		public static ImmutableSpan<Char> AsImmutableSpan(this String text, Int32 start, Int32 length) => text.AsImmutableMemory(start, length).AsImmutableSpan();

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

		public static ReadOnlySpan<Char> Trim(this ImmutableSpan<Char> span, Char trimChar) => span.AsSpan().Trim(trimChar);
		public static ReadOnlySpan<Char> Trim(this ImmutableSpan<Char> span) => span.AsSpan().Trim();
		public static ReadOnlySpan<Char> Trim(this ImmutableSpan<Char> span, ReadOnlySpan<Char> trimChars) => span.AsSpan().Trim(trimChars);

		public static ReadOnlySpan<Char> TrimEnd(this ImmutableSpan<Char> span, ReadOnlySpan<Char> trimChars) => span.AsSpan().TrimEnd(trimChars);
		public static ReadOnlySpan<Char> TrimEnd(this ImmutableSpan<Char> span) => span.AsSpan().TrimEnd();
		public static ReadOnlySpan<Char> TrimEnd(this ImmutableSpan<Char> span, Char trimChar) => span.AsSpan().TrimEnd(trimChar);

		public static ReadOnlySpan<Char> TrimStart(this ImmutableSpan<Char> span, Char trimChar) => span.AsSpan().TrimStart(trimChar);
		public static ReadOnlySpan<Char> TrimStart(this ImmutableSpan<Char> span) => span.AsSpan().TrimStart();
		public static ReadOnlySpan<Char> TrimStart(this ImmutableSpan<Char> span, ReadOnlySpan<Char> trimChars) => span.AsSpan().TrimStart(trimChars);
	}
}
