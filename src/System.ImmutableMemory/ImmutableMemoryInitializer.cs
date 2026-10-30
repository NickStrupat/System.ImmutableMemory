#if NETSTANDARD2_0
namespace System
{
	// Stand-in for System.Buffers.SpanAction<T, TArg>, which netstandard2.0 lacks. Named differently so it can't
	// collide with SpanAction when a netcoreapp consumer resolves the netstandard2.0 asset.
	public delegate void ImmutableMemoryInitializer<T, in TState>(Span<T> span, TState state);
}
#endif
