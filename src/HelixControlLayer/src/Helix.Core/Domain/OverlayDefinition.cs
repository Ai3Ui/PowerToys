namespace Helix.Core.Domain;

/// <summary>
/// User-selectable overlay bundle that can inject tools or UI chrome.
/// </summary>
public sealed record OverlayDefinition
{
    public string Id { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public IReadOnlyCollection<MatchRule> MatchRules { get; init; } = Array.Empty<MatchRule>();
    public IReadOnlyCollection<string> Tools { get; init; } = Array.Empty<string>();
    public IReadOnlyCollection<string> Modes { get; init; } = Array.Empty<string>();
    public int Priority { get; init; }
        = 0;
}
