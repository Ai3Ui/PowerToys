using Helix.Core.Domain;

namespace Helix.Orchestration.Engine;

public sealed record EngineResult
{
    public IReadOnlyList<OverlayDefinition> ActiveOverlays { get; init; } = Array.Empty<OverlayDefinition>();
    public IReadOnlyList<AutomationAction> PlannedActions { get; init; } = Array.Empty<AutomationAction>();
    public IReadOnlyDictionary<string, string> Signals { get; init; } = new Dictionary<string, string>();
    public IReadOnlyList<SessionReplayEntry> ReplayEntries { get; init; } = Array.Empty<SessionReplayEntry>();
}
