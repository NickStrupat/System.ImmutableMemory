using System;
using System.Collections.Immutable;
using Xunit;

namespace System.ImmutableMemory.Tests
{
	public class UnitTests
	{
		[Fact]
		public void TwoDifferentMemoryInstancesWithTheSameValuesAreNotEqual() // For establishing confidence in `Assert.Equal`'s behaviour.
		{
			var m1 = new []{1, 2, 3}.AsMemory();
			var m2 = new []{1, 2, 3}.AsMemory();
			Assert.NotEqual(m1, m2);
			Assert.Equal(m1, m1);
			Assert.Equal(m2, m2);
		}
		
		[Fact]
		public void ChangingUnderlyingSpanChangesReadOnlySpan()
		{
			Span<Byte> s = stackalloc Byte[1] { 42 };
			ReadOnlySpan<Byte> ros = s;

			ref readonly var r = ref ros[0];
			Assert.Equal(42, r);

			s[0] = 123;
			Assert.Equal(123, r);
		}

		[Fact]
		public void ToImmutableMemoryMakesADefensiveCopy()
		{
			var array = new Byte[42];
			var im = array.ToImmutableMemory();
			Assert.Equal(array.AsMemory(), array.AsMemory());
			Assert.NotEqual(array.AsMemory(), im.AsMemory());
		}

		[Fact]
		public void AsImmutableMemoryFromStringDoesNotMakeADefensiveCopy()
		{
			var text = "test";
			var im = text.AsImmutableMemory();
			Assert.Equal(text.AsMemory(), text.AsMemory());
			Assert.Equal(text.AsMemory(), im.AsMemory());
		}

		[Fact]
		public void AsImmutableMemoryFromImmutableArrayDoesNotMakeADefensiveCopy()
		{
			var immarr = ImmutableArray.Create(1, 2, 3);
			var im = immarr.AsImmutableMemory();
			Assert.Equal(immarr.AsMemory(), immarr.AsMemory());
			Assert.Equal(immarr.AsMemory(), im.AsMemory());
		}

		[Fact]
		public void Stackalloc()
		{
			ReadOnlySpan<Int32> span = stackalloc[] { 1, 2, 3 };
			var imm = ImmutableSpan<Int32>.UnsafeCreateWithoutCopy(span);
			Assert.True(span == imm.AsSpan());
		}

		[Fact]
		public void UnsafeCreateWithoutCopyPreservesElementType()
		{
			var source = new[] { "a", "b" };
			ImmutableSpan<String> imm = ImmutableSpan<String>.UnsafeCreateWithoutCopy(source);
			Assert.True(source.AsSpan() == imm.AsSpan());
		}

		[Fact]
		public void CreateFillsMemoryUsingState()
		{
			var im = ImmutableMemory<Int32>.Create(3, 10, (span, start) =>
			{
				for (var i = 0; i < span.Length; i++)
					span[i] = start + i;
			});
			Assert.Equal(new[] { 10, 11, 12 }, im.ToArray());
		}

		[Fact]
		public void EqualityOperatorsAreOpposites()
		{
			var text = "test";
			var whole = text.AsImmutableSpan();
			var same = text.AsImmutableSpan();
			var part = text.AsImmutableSpan(1);

			Assert.True(whole == same);
			Assert.False(whole != same);
			Assert.False(whole == part);
			Assert.True(whole != part);
		}

		[Fact]
		public void EmptyMemoryAndSpanHaveNoElements()
		{
			Assert.True(ImmutableMemory<Int32>.Empty.IsEmpty);
			Assert.Equal(0, ImmutableMemory<Int32>.Empty.Length);
			Assert.True(ImmutableSpan<Int32>.Empty.IsEmpty);
			Assert.True(new Int32[0].ToImmutableMemory().IsEmpty);
		}

		[Fact]
		public void MemorySliceSharesTheDefensiveCopy()
		{
			var im = new[] { 1, 2, 3, 4 }.ToImmutableMemory();
			var slice = im.Slice(1, 2);
			Assert.Equal(new[] { 2, 3 }, slice.ToArray());
			Assert.True(im.AsSpan().Overlaps(slice.AsSpan()));
			Assert.Equal(new[] { 3, 4 }, im.Slice(2).ToArray());
		}

		[Fact]
		public void SpanSliceAndIndexerReturnExpectedElements()
		{
			var span = "hello".AsImmutableSpan();
			Assert.Equal('e', span[1]);
			Assert.Equal("ell", span.Slice(1, 3).ToString());
			Assert.Equal("llo", span.Slice(2).ToString());
		}

		[Fact]
		public void EnumeratorVisitsEveryElementInOrder()
		{
			var visited = "";
			foreach (var c in "abc".AsImmutableSpan())
				visited += c;
			Assert.Equal("abc", visited);
		}

		[Fact]
		public void TrimExtensionsReturnTrimmedImmutableSpans()
		{
			var span = "  x  ".AsImmutableSpan();
			Assert.Equal("x", span.Trim().ToString());
			Assert.Equal("x  ", span.TrimStart().ToString());
			Assert.Equal("  x", span.TrimEnd().ToString());
			Assert.Equal("x", "--x--".AsImmutableSpan().Trim('-').ToString());
		}

		[Fact]
		public void SearchExtensionsMatchReadOnlySpanBehaviour()
		{
			var span = "abcabc".AsImmutableSpan();
			Assert.Equal(1, span.IndexOf('b'));
			Assert.Equal(4, span.LastIndexOf('b'));
			Assert.Equal(2, span.IndexOf("CA".AsSpan(), StringComparison.OrdinalIgnoreCase));
			Assert.True(span.StartsWith("abc".AsSpan()));
			Assert.True(span.EndsWith("BC".AsSpan(), StringComparison.OrdinalIgnoreCase));
			Assert.True(span.SequenceEqual("abcabc".AsSpan()));
		}

		[Fact]
		public void ImmutableArrayAndArraySegmentConversions()
		{
			var immarr = ImmutableArray.Create(1, 2, 3, 4);
			Assert.Equal(new[] { 2, 3 }, immarr.AsImmutableSpan(1, 2).ToArray());
			Assert.Equal(new[] { 3, 4 }, immarr.AsImmutableMemory(2).ToArray());

			var segment = new ArraySegment<Int32>(new[] { 0, 1, 2, 3 }, 1, 3);
			Assert.Equal(new[] { 1, 2, 3 }, segment.ToImmutableMemory().ToArray());
			Assert.Equal(new[] { 2 }, segment.ToImmutableMemory(1, 1).ToArray());
		}

		[Fact]
		public void MutatingSourceAfterCopyDoesNotChangeImmutableMemory()
		{
			var array = new[] { 1, 2, 3 };
			var im = array.ToImmutableMemory();
			array[0] = 42;
			Assert.Equal(1, im.AsSpan()[0]);
		}
	}
}
