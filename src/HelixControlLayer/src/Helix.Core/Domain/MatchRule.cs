namespace Helix.Core.Domain;

/// <summary>
/// Declarative rule that decides when an overlay or automation tool should activate.
/// </summary>
public sealed record MatchRule
{
    public string HostPattern { get; init; } = "*";
    public string? PathPattern { get; init; }
        = "/";
    public IReadOnlyCollection<string> Protocols { get; init; } = Array.Empty<string>();
    public IReadOnlyDictionary<string, string> SignalRequirements { get; init; }
        = new Dictionary<string, string>();
    public IReadOnlyCollection<string> RequiredTags { get; init; } = Array.Empty<string>();
}
