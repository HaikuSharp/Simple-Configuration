using SC.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SC;

public class ConfigurationSection(ConfigurationPath prefix, IConfiguration configuration) : ConfigurationBase(configuration.Name + configuration.Options.Separator + prefix, configuration.Options), IConfigurationSection
{
    public ConfigurationPath Prefix => prefix;

    public override IEnumerable<ConfigurationPathValuePair> Pairs => configuration.Pairs.Where(p => p.Path.IsPrefix(prefix));

    public override IEnumerable<ConfigurationPath> Paths => Pairs.Select(p => p.Path);

    public override IEnumerable<ConfigurationValue> Values => Pairs.Select(p => p.Value);

    public override bool HasSection(ConfigurationPath prefix) => configuration.HasSection(GetPath(prefix));

    public override bool HasValue(ConfigurationPath fullPath) => configuration.HasValue(GetPath(fullPath));

    public override ConfigurationValue GetValue(ConfigurationPath fullPath) => configuration.GetValue(GetPath(fullPath));

    public override void SetValue(ConfigurationPath fullPath, ConfigurationValue value) => configuration.SetValue(GetPath(fullPath), value);

    public ConfigurationPath GetPath(ConfigurationPath pathPart) => ConfigurationPath.Combine(Options.Separator, prefix, pathPart);

    public override IConfiguration Clone() => new ConfigurationSection(prefix, configuration);

    IConfiguration IConfiguration.Clone() => Clone();

    object ICloneable.Clone() => Clone();
}