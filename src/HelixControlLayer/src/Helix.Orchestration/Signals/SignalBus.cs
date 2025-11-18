namespace Helix.Orchestration.Signals;

/// <summary>
/// Lightweight in-memory signal dispatcher so overlays can share consent-aware events.
/// </summary>
public sealed class SignalBus
{
    private readonly Dictionary<string, string> _signals = new(StringComparer.OrdinalIgnoreCase);
    private readonly object _gate = new();

    public IReadOnlyDictionary<string, string> Snapshot()
    {
        lock (_gate)
        {
            return new Dictionary<string, string>(_signals, StringComparer.OrdinalIgnoreCase);
        }
    }

    public void Publish(string key, string value)
    {
        lock (_gate)
        {
            _signals[key] = value;
        }
    }
}
