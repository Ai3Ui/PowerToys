# Helix Control Layer (C#)

This standalone .NET solution models Helix as a modular, AI-orchestrated browser shell. It focuses on:

- **Modularity** – overlays, tools, and modes are stored in YAML and activated via wildcard match rules.
- **Security** – a telemetry firewall and privacy budgets keep every action consent-aware.
- **Intelligence** – a deterministic automation planner emits protocol-driven actions (Agent Mode + Markovian thinking).
- **Performance** – overlay evaluation is cached and parallelized so the CLI responds instantly even with large catalogs.

## Projects

| Project | Description |
| --- | --- |
| `Helix.Core` | Domain records for overlays, rules, telemetry, automation, and replay events. |
| `Helix.Orchestration` | Engine, rule evaluator, telemetry firewall, automation planner, YAML loader, and replay buffer. |
| `Helix.Cli` | Console front-end that loads `config/helix.yaml`, synthesizes a browser context, and prints overlays/actions. |

## Running the CLI

```
dotnet run --project src/Helix.Cli -- --config=config/helix.yaml --host=arena.pro --tags=gaming,streaming --mode=Gaming
```

All arguments use a `--key=value` syntax. Important switches:

- `--config` – path to a YAML file.
- `--host`, `--path`, `--protocol` – describe the active browser surface.
- `--tags` – comma-delimited tags like `gaming,privacy`.
- `--mode` – select a configured mode (`Privacy`, `Gaming`, `Dev`, ...).

## Configuration

The default `config/helix.yaml` showcases:

- `Privacy Shield` overlay with consent-aware match rules.
- `Gaming HUD` overlay that only lights up for tagged gaming hosts.
- `Dev Console` overlay for docs & .dev environments.
- Privacy and gaming modes with different telemetry allowances.

## Extending

1. Add overlays/modes to `config/helix.yaml`.
2. Create new tools in `Helix.Orchestration/Automation/AutomationPlanner.cs`.
3. Hook more signals or persistence providers inside the engine.
