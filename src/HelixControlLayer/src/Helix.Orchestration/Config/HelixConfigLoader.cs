using Helix.Core.Domain;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Helix.Orchestration.Config;

/// <summary>
/// Loads YAML files into strongly-typed Helix configuration records.
/// </summary>
public sealed class HelixConfigLoader
{
    private readonly IDeserializer _deserializer = new DeserializerBuilder()
        .WithNamingConvention(UnderscoredNamingConvention.Instance)
        .IgnoreUnmatchedProperties()
        .Build();

    public async Task<HelixConfig> LoadAsync(string path, CancellationToken cancellationToken = default)
    {
        await using var stream = File.OpenRead(path);
        using var reader = new StreamReader(stream);
        var yaml = await reader.ReadToEndAsync();
        cancellationToken.ThrowIfCancellationRequested();
        var config = _deserializer.Deserialize<HelixConfig>(yaml) ?? new HelixConfig();
        return config;
    }
}
