namespace Helix.Core.Domain;

/// <summary>
/// Root configuration object representing overlays, modes, and policies.
/// </summary>
public sealed record HelixConfig
{
    public IReadOnlyCollection<OverlayDefinition> Overlays { get; init; } = Array.Empty<OverlayDefinition>();
    public IReadOnlyCollection<ModeDefinition> Modes { get; init; } = Array.Empty<ModeDefinition>();
    public TelemetryPolicy Telemetry { get; init; } = new();
}
