using Helix.Core.Domain;

namespace Helix.Orchestration.Session;

/// <summary>
/// In-memory replay buffer that truncates automatically to respect privacy budgets.
/// </summary>
public sealed class SessionReplayBuffer
{
    private readonly int _capacity;
    private readonly LinkedList<SessionReplayEntry> _entries = new();
    private readonly object _gate = new();

    public SessionReplayBuffer(int capacity = 128)
    {
        _capacity = capacity;
    }

    public void Add(SessionReplayEntry entry)
    {
        lock (_gate)
        {
            _entries.AddLast(entry);
            while (_entries.Count > _capacity)
            {
                _entries.RemoveFirst();
            }
        }
    }

    public IReadOnlyList<SessionReplayEntry> Snapshot()
    {
        lock (_gate)
        {
            return _entries.ToList();
        }
    }
}
