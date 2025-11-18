namespace Helix.Core.Domain;

/// <summary>
/// Consent-aware telemetry constraints enforced before automation executes.
/// </summary>
public sealed record TelemetryPolicy
{
    public bool AllowTelemetry { get; init; }
        = false;
    public bool AllowSpoofing { get; init; } = false;
    public bool RequireExplicitConsent { get; init; } = true;
    public IReadOnlyCollection<string> AllowedDestinations { get; init; } = Array.Empty<string>();
}
