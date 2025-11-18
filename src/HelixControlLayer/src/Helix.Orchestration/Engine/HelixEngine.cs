using Helix.Core.Domain;
using Helix.Orchestration.Automation;
using Helix.Orchestration.Privacy;
using Helix.Orchestration.Rules;
using Helix.Orchestration.Session;
using Helix.Orchestration.Signals;

namespace Helix.Orchestration.Engine;

/// <summary>
/// Coordinates match rules, telemetry firewalls, and automation planning.
/// </summary>
public sealed class HelixEngine
{
    private readonly HelixConfig _config;
    private readonly MatchRuleEvaluator _matchEvaluator;
    private readonly TelemetryFirewall _firewall;
    private readonly AutomationPlanner _automationPlanner;
    private readonly SessionReplayBuffer _replayBuffer;
    private readonly SignalBus _signalBus;

    public HelixEngine(
        HelixConfig config,
        MatchRuleEvaluator? matchEvaluator = null,
        TelemetryFirewall? firewall = null,
        AutomationPlanner? automationPlanner = null,
        SessionReplayBuffer? replayBuffer = null,
        SignalBus? signalBus = null)
    {
        _config = config;
        _matchEvaluator = matchEvaluator ?? new MatchRuleEvaluator();
        _firewall = firewall ?? new TelemetryFirewall();
        _automationPlanner = automationPlanner ?? new AutomationPlanner();
        _replayBuffer = replayBuffer ?? new SessionReplayBuffer();
        _signalBus = signalBus ?? new SignalBus();
    }

    public async Task<EngineResult> ProcessAsync(BrowserContext context, string? modeName, CancellationToken cancellationToken = default)
    {
        var mode = _config.Modes.FirstOrDefault(m => string.Equals(m.Name, modeName, StringComparison.OrdinalIgnoreCase));
        var firewallResult = _firewall.Evaluate(_config, mode);
        var overlays = await FilterOverlaysAsync(context, mode, cancellationToken);
        var actions = _automationPlanner.BuildPlan(overlays, context);

        if (firewallResult.SessionReplayAllowed)
        {
            foreach (var overlay in overlays)
            {
                _replayBuffer.Add(new SessionReplayEntry
                {
                    OverlayId = overlay.Id,
                    EventType = "overlay-activated",
                    Payload = context.Host,
                });
            }
        }

        foreach (var overlay in overlays)
        {
            _signalBus.Publish($"overlay::{overlay.Id}", "active");
        }

        var signals = _signalBus.Snapshot();

        return new EngineResult
        {
            ActiveOverlays = overlays,
            PlannedActions = actions,
            Signals = signals,
            ReplayEntries = firewallResult.SessionReplayAllowed ? _replayBuffer.Snapshot() : Array.Empty<SessionReplayEntry>()
        };
    }

    private Task<IReadOnlyList<OverlayDefinition>> FilterOverlaysAsync(BrowserContext context, ModeDefinition? mode, CancellationToken cancellationToken)
    {
        var overlays = _config.Overlays
            .AsParallel()
            .WithCancellation(cancellationToken)
            .Where(overlay => _matchEvaluator.IsMatch(overlay, context, mode))
            .OrderByDescending(overlay => overlay.Priority)
            .ToList();

        return Task.FromResult<IReadOnlyList<OverlayDefinition>>(overlays);
    }
}
