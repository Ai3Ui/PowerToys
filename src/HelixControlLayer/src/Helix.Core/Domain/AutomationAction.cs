namespace Helix.Core.Domain;

/// <summary>
/// Defines a deterministic automation step executed by Agent Mode.
/// </summary>
public sealed record AutomationAction
{
    public string Name { get; init; } = string.Empty;
    public string Protocol { get; init; } = string.Empty;
    public IReadOnlyDictionary<string, string> Parameters { get; init; } = new Dictionary<string, string>();
}
