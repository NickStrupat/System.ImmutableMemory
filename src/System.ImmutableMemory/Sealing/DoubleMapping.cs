#if NET
using System.Runtime.InteropServices;

namespace System.Sealing
{
	// The same physical pages mapped twice: once writable, for filling, and once read-only, for handing out. Unmapping
	// the writable view leaves the read-only view, and the pages behind it, intact.
	internal readonly struct DoubleMapping
	{
		public readonly IntPtr Writable;
		public readonly IntPtr ReadOnly;
		public readonly nuint Size;

		private DoubleMapping(IntPtr writable, IntPtr readOnly, nuint size)
		{
			Writable = writable;
			ReadOnly = readOnly;
			Size = size;
		}

		public static Boolean IsSupported => OperatingSystem.IsMacOS() || OperatingSystem.IsLinux() || OperatingSystem.IsWindows();

		public static DoubleMapping Create(nuint size)
		{
			if (OperatingSystem.IsMacOS())
				return Mac.Create(size);
			if (OperatingSystem.IsLinux())
				return Linux.Create(size);
			if (OperatingSystem.IsWindows())
				return Windows.Create(size);
			throw new PlatformNotSupportedException();
		}

		public void UnmapWritable() => Unmap(Writable, Size);
		public void UnmapReadOnly() => Unmap(ReadOnly, Size);

		private static void Unmap(IntPtr address, nuint size)
		{
			if (OperatingSystem.IsWindows())
				Windows.Unmap(address);
			else
				Posix.Unmap(address, size);
		}

		private static class Posix
		{
			public const Int32 ProtRead = 1;
			public const Int32 ProtWrite = 2;
			public const Int32 MapShared = 1;
			public static readonly IntPtr MapFailed = new IntPtr(-1);

			[DllImport("libc", SetLastError = true)]
			public static extern IntPtr mmap(IntPtr address, nuint length, Int32 prot, Int32 flags, Int32 fd, Int64 offset);

			[DllImport("libc", SetLastError = true)]
			private static extern Int32 munmap(IntPtr address, nuint length);

			public static IntPtr Map(nuint size, Int32 prot, Int32 flags, Int32 fd)
			{
				var address = mmap(IntPtr.Zero, size, prot, flags, fd, 0);
				if (address == MapFailed)
					throw new OutOfMemoryException($"mmap failed with errno {Marshal.GetLastPInvokeError()}.");
				return address;
			}

			public static void Unmap(IntPtr address, nuint size)
			{
				if (munmap(address, size) != 0)
					throw new InvalidOperationException($"munmap failed with errno {Marshal.GetLastPInvokeError()}.");
			}
		}

		// Anonymous shared memory, aliased with mach_vm_remap. The alias's maximum protection is lowered to read-only,
		// so not even mprotect can make it writable again.
		private static class Mac
		{
			private const Int32 MapAnonymous = 0x1000;
			private const Int32 VmFlagsAnywhere = 1;
			private const Int32 VmProtRead = 1;
			private const UInt32 VmInheritNone = 2;
			private const Int32 KernSuccess = 0;

			private static readonly UInt32 taskSelf = (UInt32)Marshal.ReadInt32(
				NativeLibrary.GetExport(NativeLibrary.Load("libc"), "mach_task_self_"));

			[DllImport("libc")]
			private static extern Int32 mach_vm_remap(UInt32 targetTask, ref UInt64 targetAddress, UInt64 size,
				UInt64 mask, Int32 flags, UInt32 sourceTask, UInt64 sourceAddress, Int32 copy,
				out Int32 currentProtection, out Int32 maximumProtection, UInt32 inheritance);

			[DllImport("libc")]
			private static extern Int32 mach_vm_protect(UInt32 task, UInt64 address, UInt64 size, Int32 setMaximum,
				Int32 newProtection);

			public static DoubleMapping Create(nuint size)
			{
				var writable = Posix.Map(size, Posix.ProtRead | Posix.ProtWrite, Posix.MapShared | MapAnonymous, -1);
				UInt64 readOnly = 0;
				Check(mach_vm_remap(taskSelf, ref readOnly, size, 0, VmFlagsAnywhere, taskSelf, (UInt64)writable, 0,
					out _, out _, VmInheritNone), "mach_vm_remap");
				Check(mach_vm_protect(taskSelf, readOnly, size, 1, VmProtRead), "mach_vm_protect (maximum)");
				Check(mach_vm_protect(taskSelf, readOnly, size, 0, VmProtRead), "mach_vm_protect (current)");
				return new DoubleMapping(writable, (IntPtr)readOnly, size);
			}

			private static void Check(Int32 result, String call)
			{
				if (result != KernSuccess)
					throw new OutOfMemoryException($"{call} failed with kern_return_t {result}.");
			}
		}

		// An anonymous memfd mapped twice. F_SEAL_FUTURE_WRITE (Linux 5.1+) is added between the two mmaps: the
		// writable view, mapped before it, stays writable, while the read-only view, mapped after it, can't be
		// mprotect-ed writable, and no new writable mapping or write() is allowed. The fd is closed once both views exist.
		private static class Linux
		{
			private const UInt32 MfdCloexec = 0x1;
			private const UInt32 MfdAllowSealing = 0x2;
			private const Int32 FAddSeals = 1024 + 9;
			private const Int32 FSealSeal = 0x1;
			private const Int32 FSealShrink = 0x2;
			private const Int32 FSealGrow = 0x4;
			private const Int32 FSealFutureWrite = 0x10;

			[DllImport("libc", SetLastError = true)]
			private static extern Int32 memfd_create(String name, UInt32 flags);

			[DllImport("libc", SetLastError = true)]
			private static extern Int32 ftruncate(Int32 fd, Int64 length);

			[DllImport("libc", SetLastError = true)]
			private static extern Int32 close(Int32 fd);

			// fcntl is variadic; on x64 and arm64 Linux an int vararg is passed like a fixed int argument.
			[DllImport("libc", SetLastError = true)]
			private static extern Int32 fcntl(Int32 fd, Int32 command, Int32 argument);

			public static DoubleMapping Create(nuint size)
			{
				var fd = memfd_create("ImmutableMemory", MfdCloexec | MfdAllowSealing);
				if (fd < 0)
					throw new OutOfMemoryException($"memfd_create failed with errno {Marshal.GetLastPInvokeError()}.");
				try
				{
					if (ftruncate(fd, (Int64)size) != 0)
						throw new OutOfMemoryException($"ftruncate failed with errno {Marshal.GetLastPInvokeError()}.");
					var writable = Posix.Map(size, Posix.ProtRead | Posix.ProtWrite, Posix.MapShared, fd);
					try
					{
						if (fcntl(fd, FAddSeals, FSealFutureWrite | FSealShrink | FSealGrow | FSealSeal) != 0)
							throw new PlatformNotSupportedException(
								$"Sealing the memfd failed with errno {Marshal.GetLastPInvokeError()}; F_SEAL_FUTURE_WRITE needs Linux 5.1 or later.");
						var readOnly = Posix.Map(size, Posix.ProtRead, Posix.MapShared, fd);
						return new DoubleMapping(writable, readOnly, size);
					}
					catch
					{
						Posix.Unmap(writable, size);
						throw;
					}
				}
				finally
				{
					close(fd);
				}
			}
		}

		// A pagefile-backed section mapped twice. A FILE_MAP_READ view can't be VirtualProtect-ed to writable.
		private static class Windows
		{
			private static readonly IntPtr invalidHandleValue = new IntPtr(-1);
			private const UInt32 PageReadWrite = 0x04;
			private const UInt32 FileMapWrite = 0x02;
			private const UInt32 FileMapRead = 0x04;

			[DllImport("kernel32", SetLastError = true)]
			private static extern IntPtr CreateFileMappingW(IntPtr file, IntPtr attributes, UInt32 protect,
				UInt32 maximumSizeHigh, UInt32 maximumSizeLow, IntPtr name);

			[DllImport("kernel32", SetLastError = true)]
			private static extern IntPtr MapViewOfFile(IntPtr mapping, UInt32 access, UInt32 offsetHigh,
				UInt32 offsetLow, nuint bytes);

			[DllImport("kernel32", SetLastError = true)]
			private static extern Boolean UnmapViewOfFile(IntPtr address);

			[DllImport("kernel32", SetLastError = true)]
			private static extern Boolean CloseHandle(IntPtr handle);

			public static DoubleMapping Create(nuint size)
			{
				var section = CreateFileMappingW(invalidHandleValue, IntPtr.Zero, PageReadWrite,
					(UInt32)((UInt64)size >> 32), (UInt32)size, IntPtr.Zero);
				if (section == IntPtr.Zero)
					throw new OutOfMemoryException($"CreateFileMapping failed with error {Marshal.GetLastPInvokeError()}.");
				try
				{
					var writable = MapView(section, FileMapWrite, size);
					var readOnly = MapView(section, FileMapRead, size);
					return new DoubleMapping(writable, readOnly, size);
				}
				finally
				{
					CloseHandle(section);
				}
			}

			public static void Unmap(IntPtr address)
			{
				if (!UnmapViewOfFile(address))
					throw new InvalidOperationException($"UnmapViewOfFile failed with error {Marshal.GetLastPInvokeError()}.");
			}

			private static IntPtr MapView(IntPtr section, UInt32 access, nuint size)
			{
				var view = MapViewOfFile(section, access, 0, 0, size);
				if (view == IntPtr.Zero)
					throw new OutOfMemoryException($"MapViewOfFile failed with error {Marshal.GetLastPInvokeError()}.");
				return view;
			}
		}
	}
}
#endif
