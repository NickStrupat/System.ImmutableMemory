using System;
using Xunit;

namespace Tests
{
	public class UnitTest1
	{
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
		public void ToImmutableMemoryMakesACopy()
		{
			var array = new Byte[42];
			var im = array.ToImmutableMemory();
			Assert.Equal(array.AsMemory(), array.AsMemory());
			Assert.NotEqual(array.AsMemory(), im.AsMemory());
		}

		[Fact]
		public void AsImmutableMemoryFromStringDoesNotMakeACopy()
		{
			var text = "test";
			var im = text.AsImmutableMemory();
			Assert.Equal(text.AsMemory(), text.AsMemory());
			Assert.Equal(text.AsMemory(), im.AsMemory());
		}
	}
}
