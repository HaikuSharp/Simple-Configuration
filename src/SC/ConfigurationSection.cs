using SC.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SC;

public sealed class ConfigurationSection(ConfigurationPath prefix, IConfiguration configuration) : ConfigurationBase(configuration.Name + configuration.Options.Separator + prefix, configuration.Options), IConfigurationSection
{
    public ConfigurationPath Prefix => prefix;

    public override IEnumerable<ConfigurationPathValuePair> Pairs => configuration.Pairs.Where(p => p.Path.IsPrefix(prefix));

    public override IEnumerable<ConfigurationPath> Paths => Pairs.Select(p => p.Path);

    public override IEnumerable<ConfigurationValue> Values => Pairs.Select(p => p.Value);

    public override bool HasSection(ConfigurationPath prefix) => configuration.HasSection(GetAbsolutePath(prefix));

    public override bool HasValue(ConfigurationPath localPath) => configuration.HasValue(GetAbsolutePath(localPath));

    public override ConfigurationValue GetValue(ConfigurationPath localPath) => configuration.GetValue(GetAbsolutePath(localPath));

    public override void SetValue(ConfigurationPath localPath, ConfigurationValue value) => configuration.SetValue(GetAbsolutePath(localPath), value);

    public override void Add(ConfigurationPath localPath, ConfigurationValue value) => configuration.Add(GetAbsolutePath(localPath), value);

    public override void Remove(ConfigurationPath localPath) => configuration.Remove(GetAbsolutePath(localPath));

    public ConfigurationPath GetAbsolutePath(ConfigurationPath pathPart) => ConfigurationPath.Combine(Options.Separator, prefix, pathPart);

    public override IConfiguration Clone() => new ConfigurationSection(prefix, configuration);

    object ICloneable.Clone() => Clone();
}