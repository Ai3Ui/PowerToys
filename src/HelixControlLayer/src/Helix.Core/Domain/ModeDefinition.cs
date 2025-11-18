namespace Helix.Core.Domain;

/// <summary>
/// A named operating mode (Privacy, Gaming, Dev) with overrides for policies.
/// </summary>
public sealed record ModeDefinition
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public IReadOnlyDictionary<string, string> Signals { get; init; } = new Dictionary<string, string>();
    public PrivacyBudget Budget { get; init; } = PrivacyBudget.Empty;
}

public sealed record PrivacyBudget
{
    public static PrivacyBudget Empty { get; } = new();

    public double TelemetrySamplingRate { get; init; } = 0;
    public bool AllowSessionReplay { get; init; }
        = false;
    public bool AllowExternalCalls { get; init; } = false;
}
