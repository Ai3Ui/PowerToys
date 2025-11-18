using Helix.Core.Domain;
using Helix.Orchestration.Config;
using Helix.Orchestration.Engine;

namespace Helix.Cli;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var options = ParseArgs(args);
        var configPath = options.TryGetValue("config", out var providedConfig)
            ? providedConfig
            : Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "config", "helix.yaml");

        if (!File.Exists(configPath))
        {
            Console.Error.WriteLine($"Unable to find configuration at {configPath}.");
            return;
        }

        var loader = new HelixConfigLoader();
        var config = await loader.LoadAsync(configPath);
        var engine = new HelixEngine(config);
        var context = CreateContext(options);
        var mode = options.GetValueOrDefault("mode", "Privacy");
        var result = await engine.ProcessAsync(context, mode);

        Console.WriteLine($"Helix mode: {mode}");
        Console.WriteLine("Active overlays:");
        foreach (var overlay in result.ActiveOverlays)
        {
            Console.WriteLine($" - {overlay.Title} :: {overlay.Description}");
        }

        Console.WriteLine();
        Console.WriteLine("Planned automation actions:");
        foreach (var action in result.PlannedActions)
        {
            Console.WriteLine($" - {action.Name} via {action.Protocol}");
        }

        Console.WriteLine();
        Console.WriteLine("Signal snapshot:");
        foreach (var kvp in result.Signals)
        {
            Console.WriteLine($" - {kvp.Key} = {kvp.Value}");
        }

        if (result.ReplayEntries.Count > 0)
        {
            Console.WriteLine();
            Console.WriteLine("Session replay entries:");
            foreach (var replay in result.ReplayEntries)
            {
                Console.WriteLine($"[{replay.Timestamp:u}] {replay.OverlayId} -> {replay.EventType}");
            }
        }
    }

    private static BrowserContext CreateContext(Dictionary<string, string> options)
    {
        var signals = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["consent"] = options.GetValueOrDefault("consent", "granted"),
            ["persona"] = options.GetValueOrDefault("persona", "researcher"),
        };

        var tags = options.TryGetValue("tags", out var tagsCsv)
            ? tagsCsv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            : Array.Empty<string>();

        return new BrowserContext(
            Host: options.GetValueOrDefault("host", "example.com"),
            Path: options.GetValueOrDefault("path", "/"),
            Protocol: options.GetValueOrDefault("protocol", "https"),
            Tags: tags,
            Signals: signals,
            IsOffline: bool.TryParse(options.GetValueOrDefault("offline", "false"), out var offline) && offline);
    }

    private static Dictionary<string, string> ParseArgs(string[] args)
    {
        var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var arg in args)
        {
            if (!arg.StartsWith("--", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var parts = arg[2..].Split('=', 2, StringSplitOptions.TrimEntries);
            if (parts.Length == 2)
            {
                dict[parts[0]] = parts[1];
            }
        }

        return dict;
    }
}
