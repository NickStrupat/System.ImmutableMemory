#if NETSTANDARD2_0
namespace System
{
	/// <summary>Fills a newly allocated span for <see cref="ImmutableMemory{T}.Create{TState}"/>.</summary>
	/// <remarks>
	/// Stand-in for <c>System.Buffers.SpanAction&lt;T, TArg&gt;</c>, which netstandard2.0 lacks. It is named differently
	/// so it can't collide with <c>SpanAction</c> when a netcoreapp consumer resolves the netstandard2.0 asset.
	/// </remarks>
	/// <param name="span">The span to fill.</param>
	/// <param name="state">The state passed to <see cref="ImmutableMemory{T}.Create{TState}"/>.</param>
	/// <typeparam name="T">The type of the elements.</typeparam>
	/// <typeparam name="TState">The type of <paramref name="state"/>.</typeparam>
	public delegate void ImmutableMemoryInitializer<T, in TState>(Span<T> span, TState state);
}
#endif
