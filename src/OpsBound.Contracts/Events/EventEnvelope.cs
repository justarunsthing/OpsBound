namespace OpsBound.Contracts.Events;

/// <summary>
/// CloudEvents-style JSON envelope. Wire format is camelCase: <c>id</c>, <c>type</c>, <c>source</c>, <c>time</c>, <c>data</c>.
/// </summary>
public sealed record EventEnvelope<TData>
{
    public required string Id { get; init; }

    public required string Type { get; init; }

    public required string Source { get; init; }

    public required DateTimeOffset Time { get; init; }

    public required TData Data { get; init; }
}
