# ImmutableMemory

Immutable versions of `Memory&lt;T&gt;` and `Span&lt;T&gt;`, because `ReadOnlyMemory/Span` doesn't imply the underlying memory won't be mutated. These types either wrap known immutable objects (`String`, `ImmutableArray&lt;T&gt;`), or they take sole ownership of a defensive copy of the underlying memory. They then provide a non-mutating API which closely resembles `ReadOnlyMemory/Span`.