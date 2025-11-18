# Helix Control Layer Architecture

## Flow
1. **Config Loader** – reads YAML overlays/modes into strongly typed records.
2. **Helix Engine** – parallel rule evaluation + telemetry firewall.
3. **Automation Planner** – transforms overlays into deterministic actions.
4. **Signal Bus & Replay Buffer** – capture provenance + transparency events.

## Making it faster
- Wildcard patterns compiled once and cached per rule.
- Overlay evaluation uses PLINQ (`AsParallel`) to scale across cores.
- Replay buffer + signal bus are lock-minimized to avoid blocking UI threads.

## Security & Transparency
- Telemetry firewall calculates a `TelemetryFirewallResult` so the caller knows whether to persist replay data.
- Session replay truncates automatically to stay within privacy budgets.
- Signals share overlay activation status without exposing browsing history.
