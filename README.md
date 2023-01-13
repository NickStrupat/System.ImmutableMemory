# ImmutableMemory

Immutable versions of `Memory<T>` and `Span<T>`, because `ReadOnlyMemory/Span` doesn't imply the underlying memory won't be mutated. These types either wrap known immutable objects (String, Immutable...<T>` or they take sole ownership of a defensive copy of the underlying memory. They then provide a non-mutating API which closely resembles `ReadOnlyMemory/Span`.