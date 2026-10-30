using System;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
	}
}
