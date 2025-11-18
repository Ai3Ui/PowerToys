namespace Helix.Core.Domain;

/// <summary>
/// Describes the current browser tab or surface that Helix is augmenting.
/// </summary>
public sealed record BrowserContext(
    string Host,
    string Path,
    string Protocol,
    IReadOnlyCollection<string> Tags,
    IReadOnlyDictionary<string, string> Signals,
    bool IsOffline)
{
    public static BrowserContext Empty { get; } = new(
        Host: string.Empty,
        Path: string.Empty,
        Protocol: "https",
        Tags: Array.Empty<string>(),
        Signals: new Dictionary<string, string>(),
        IsOffline: false);
}
