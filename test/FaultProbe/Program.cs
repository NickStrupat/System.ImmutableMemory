using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

var mode = args[0];
var size = args.Length > 1 ? Int32.Parse(args[1]) : 4;
var source = new Int32[size];

// "copy" is the control: an ordinary defensive copy, which these writes succeed against.
var sealedMemory = mode.StartsWith("copy") ? null : SealedMemory<Int32>.Create(source);
var memory = sealedMemory?.Memory ?? source.ToImmutableMemory();

Console.WriteLine("before write");
switch (mode.Replace("copy-", ""))
{
	case "asmemory":
		MemoryMarshal.AsMemory(memory.AsMemory()).Span[0] = 42;
		break;
	case "asref":
		Unsafe.AsRef(in MemoryMarshal.GetReference(memory.AsSpan())) = 42;
		break;
	case "pin":
		unsafe
		{
			using var handle = memory.Pin();
			*(Int32*)handle.Pointer = 42;
		}
		break;
	case "manager":
		MemoryMarshal.TryGetMemoryManager<Int32, MemoryManager<Int32>>(memory.AsMemory(), out var manager);
		manager!.GetSpan()[0] = 42;
		break;
	default:
		throw new ArgumentException($"Unknown mode '{mode}'.");
}
Console.WriteLine($"after write: {memory.AsSpan()[0]}");
