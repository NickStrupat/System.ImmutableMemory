# ImmutableMemory

Immutable versions of `Memory<T>` and `Span<T>`, because `ReadOnlyMemory/Span` doesn't imply the underlying memory won't be mutated. These types either wrap known immutable objects (`String`, `ImmutableArray<T>`), or they take sole ownership of a defensive copy of the underlying memory. They then provide a non-mutating API which closely resembles `ReadOnlyMemory/Span`.

## Usage

```csharp
var bytes = new Byte[] { 1, 2, 3 };
ImmutableMemory<Byte> copy = bytes.ToImmutableMemory(); // defensive copy
bytes[0] = 42;                                          // `copy` still starts with 1

ImmutableMemory<Char> text = "hello".AsImmutableMemory();       // no copy; strings are immutable
ImmutableSpan<Int32> items = ImmutableArray.Create(1, 2, 3).AsImmutableSpan(); // no copy

ImmutableSpan<Char> trimmed = "  hi  ".AsImmutableSpan().Trim(); // MemoryExtensions-style API
```

`ToImmutableMemory()` always copies. `AsImmutableMemory()`/`AsImmutableSpan()` never copy, and only exist for sources that are already immutable.

## Equality

Like `ReadOnlyMemory<T>`/`ReadOnlySpan<T>`, equality compares *which memory* is referenced, not its contents. Two separate copies of the same elements are not equal; use `SequenceEqual` to compare contents.

## Limits of the guarantee

The guarantee holds as long as nothing bypasses the type system. These can still mutate the underlying memory:

- `MemoryMarshal` on the result of `AsMemory()`/`AsSpan()` (e.g. `MemoryMarshal.TryGetArray`)
- Writing through pointers from `Pin()` or `fixed`
- `ImmutableSpan<T>.UnsafeCreateWithoutCopy`, when anything else can still write to the source

## Target frameworks

`netstandard2.0` and `net8.0`. On `netstandard2.0`, `ImmutableMemory<T>.Create` takes an `ImmutableMemoryInitializer<T, TState>` instead of `SpanAction<T, TState>`; lambdas work with either.
