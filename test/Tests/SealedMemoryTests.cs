using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Xunit;

namespace System.ImmutableMemory.Tests
{
	public class SealedMemoryTests
	{
		[Fact]
		public void CreateCopiesContents()
		{
			using var sealedMemory = SealedMemory<Int32>.Create(new[] { 1, 2, 3 });
			Assert.Equal(new[] { 1, 2, 3 }, sealedMemory.Memory.ToArray());
		}

		[Fact]
		public void EmptySourceGivesEmptyMemory()
		{
			using var sealedMemory = SealedMemory<Int32>.Create(ReadOnlySpan<Int32>.Empty);
			Assert.True(sealedMemory.Memory.IsEmpty);
		}

		[Fact]
		public void TryGetArrayFindsNoArray()
		{
			using var sealedMemory = SealedMemory<Int32>.Create(new[] { 1, 2, 3 });
			Assert.False(MemoryMarshal.TryGetArray(sealedMemory.Memory.AsMemory(), out _));
		}

		[Fact]
		public unsafe void SmallBuffersSharePages()
		{
			var buffers = Enumerable.Range(0, 10).Select(i => SealedMemory<Int32>.Create(new[] { i })).ToList();
			try
			{
				var pages = buffers.Select(b =>
				{
					using var handle = b.Memory.Pin();
					return (UInt64)handle.Pointer / (UInt64)Environment.SystemPageSize;
				});
				Assert.True(pages.Distinct().Count() <= 2); // 2 in case a chunk filled up mid-way
			}
			finally
			{
				buffers.ForEach(b => b.Dispose());
			}
		}

		[Fact]
		public void LargeBufferRoundTrips()
		{
			var source = Enumerable.Range(0, 100_000).ToArray();
			using var sealedMemory = SealedMemory<Int32>.Create(source);
			Assert.True(sealedMemory.Memory.AsSpan().SequenceEqual(source));
		}

		[Fact]
		public void MemoryTakenBeforeDisposeThrowsAfterwards()
		{
			var sealedMemory = SealedMemory<Int32>.Create(new[] { 1, 2, 3 });
			var memory = sealedMemory.Memory;
			sealedMemory.Dispose();

			Assert.Throws<ObjectDisposedException>(() => sealedMemory.Memory);
			Assert.Throws<ObjectDisposedException>(() => memory.AsSpan().Length);
		}

		[Fact]
		public unsafe void ReadOnlyViewCannotBeMadeWritable()
		{
			using var sealedMemory = SealedMemory<Int32>.Create(new[] { 1, 2, 3 });
			using var handle = sealedMemory.Memory.Pin();
			var pageSize = (UInt64)Environment.SystemPageSize;
			var page = (IntPtr)((UInt64)handle.Pointer / pageSize * pageSize);

			if (OperatingSystem.IsWindows())
				Assert.False(VirtualProtect(page, (nuint)pageSize, PageReadWrite, out _));
			else
				Assert.Equal(-1, mprotect(page, (nuint)pageSize, ProtRead | ProtWrite));
		}

		[Theory]
		[InlineData("asmemory", 4)]
		[InlineData("asref", 4)]
		[InlineData("pin", 4)]
		[InlineData("manager", 4)]
		[InlineData("asmemory", 100_000)] // large buffers get their own chunk
		public void WritingToSealedMemoryKillsTheProcess(String mode, Int32 size)
		{
			var (exitCode, output) = RunProbe(mode, size);
			Assert.NotEqual(0, exitCode);
			Assert.Contains("before write", output);
			Assert.DoesNotContain("after write", output);
			Assert.Contains("AccessViolationException", output);
		}

		[Fact]
		public void WritingToAnOrdinaryCopySucceeds() // the control: what SealedMemory closes off
		{
			var (exitCode, output) = RunProbe("copy-asmemory", 4);
			Assert.Equal(0, exitCode);
			Assert.Contains("after write: 42", output);
		}

		private static (Int32 ExitCode, String Output) RunProbe(String mode, Int32 size)
		{
			var info = new ProcessStartInfo(Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? "dotnet")
			{
				RedirectStandardOutput = true,
				RedirectStandardError = true,
			};
			info.ArgumentList.Add(Path.Combine(AppContext.BaseDirectory, "FaultProbe.dll"));
			info.ArgumentList.Add(mode);
			info.ArgumentList.Add(size.ToString());
			info.Environment["DOTNET_DbgEnableMiniDump"] = "0";

			using var process = Process.Start(info)!;
			var stderr = process.StandardError.ReadToEndAsync();
			var stdout = process.StandardOutput.ReadToEnd();
			process.WaitForExit();
			return (process.ExitCode, stdout + stderr.Result);
		}

		private const Int32 ProtRead = 1;
		private const Int32 ProtWrite = 2;
		private const UInt32 PageReadWrite = 0x04;

		[DllImport("libc", SetLastError = true)]
		private static extern Int32 mprotect(IntPtr address, nuint length, Int32 prot);

		[DllImport("kernel32", SetLastError = true)]
		private static extern Boolean VirtualProtect(IntPtr address, nuint size, UInt32 newProtect, out UInt32 oldProtect);
	}
}
