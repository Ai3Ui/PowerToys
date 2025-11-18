namespace Helix.Core.Domain;

/// <summary>
/// Lightweight replay entry that is persisted only when privacy budget allows.
/// </summary>
public sealed record SessionReplayEntry
{
    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;
    public string OverlayId { get; init; } = string.Empty;
    public string EventType { get; init; } = string.Empty;
    public string Payload { get; init; } = string.Empty;
}
