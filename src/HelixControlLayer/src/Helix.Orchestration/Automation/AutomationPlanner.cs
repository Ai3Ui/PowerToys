using Helix.Core.Domain;

namespace Helix.Orchestration.Automation;

/// <summary>
/// Builds deterministic action plans that can later be executed by a headless agent.
/// </summary>
public sealed class AutomationPlanner
{
    public IReadOnlyList<AutomationAction> BuildPlan(IEnumerable<OverlayDefinition> overlays, BrowserContext context)
    {
        var actions = new List<AutomationAction>();
        foreach (var overlay in overlays)
        {
            foreach (var tool in overlay.Tools)
            {
                actions.Add(CreateAction(tool, overlay, context));
            }
        }

        return actions;
    }

    private static AutomationAction CreateAction(string tool, OverlayDefinition overlay, BrowserContext context)
    {
        var parameters = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["overlay"] = overlay.Id,
            ["host"] = context.Host,
            ["path"] = context.Path,
        };

        if (tool.Contains("privacy", StringComparison.OrdinalIgnoreCase))
        {
            parameters["level"] = "strict";
        }
        else if (tool.Contains("gaming", StringComparison.OrdinalIgnoreCase))
        {
            parameters["latencyBudgetMs"] = "30";
        }
        else if (tool.Contains("dev", StringComparison.OrdinalIgnoreCase))
        {
            parameters["instrumentation"] = "detailed";
        }

        return new AutomationAction
        {
            Name = tool,
            Protocol = tool.Contains("ai", StringComparison.OrdinalIgnoreCase) ? "agent" : "overlay",
            Parameters = parameters,
        };
    }
}
