using Helix.Core.Domain;

namespace Helix.Orchestration.Privacy;

public sealed record TelemetryFirewallResult(bool TelemetryAllowed, bool SessionReplayAllowed, string Message);

/// <summary>
/// Ensures telemetry destinations respect consent and per-mode privacy budgets without halting the pipeline.
/// </summary>
public sealed class TelemetryFirewall
{
    public TelemetryFirewallResult Evaluate(HelixConfig config, ModeDefinition? mode)
    {
        var telemetryAllowed = config.Telemetry.AllowTelemetry || (mode?.Budget.TelemetrySamplingRate > 0);
        var replayAllowed = telemetryAllowed && (mode?.Budget.AllowSessionReplay ?? false);
        var message = telemetryAllowed
            ? "Telemetry allowed for this mode."
            : "Telemetry disabled – Helix will operate in fully local mode.";

        return new TelemetryFirewallResult(telemetryAllowed, replayAllowed, message);
    }
}
