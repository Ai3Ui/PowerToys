using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using Helix.Core.Domain;

namespace Helix.Orchestration.Rules;

/// <summary>
/// Fast pattern matcher with caching so overlays can be evaluated in parallel.
/// </summary>
public sealed class MatchRuleEvaluator
{
    private readonly ConcurrentDictionary<string, Regex> _patternCache = new(StringComparer.OrdinalIgnoreCase);

    public bool IsMatch(OverlayDefinition overlay, BrowserContext context, ModeDefinition? activeMode)
        => overlay.MatchRules.Count == 0 || overlay.MatchRules.Any(rule => IsMatch(rule, context, activeMode));

    private bool IsMatch(MatchRule rule, BrowserContext context, ModeDefinition? mode)
    {
        var hostMatches = MatchWildcard(rule.HostPattern, context.Host);
        var pathMatches = string.IsNullOrEmpty(rule.PathPattern) || MatchWildcard(rule.PathPattern!, context.Path);
        var protocolMatches = rule.Protocols.Count == 0 || rule.Protocols.Contains(context.Protocol, StringComparer.OrdinalIgnoreCase);
        var tagsSatisfied = !rule.RequiredTags.Except(context.Tags, StringComparer.OrdinalIgnoreCase).Any();
        var signalsSatisfied = rule.SignalRequirements.All(req =>
            context.Signals.TryGetValue(req.Key, out var value) && string.Equals(value, req.Value, StringComparison.OrdinalIgnoreCase));
        var modeSatisfied = mode is null || !mode.Signals.Any() || mode.Signals.All(pair =>
            context.Signals.TryGetValue(pair.Key, out var signalValue) && string.Equals(signalValue, pair.Value, StringComparison.OrdinalIgnoreCase));

        return hostMatches && pathMatches && protocolMatches && tagsSatisfied && signalsSatisfied && modeSatisfied;
    }

    private bool MatchWildcard(string pattern, string value)
    {
        if (pattern == "*")
        {
            return true;
        }

        var regex = _patternCache.GetOrAdd(pattern, static key =>
        {
            var escaped = Regex.Escape(key).Replace("\\*", ".*").Replace("\\?", ".");
            return new Regex($"^{escaped}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        });

        return regex.IsMatch(value);
    }
}
