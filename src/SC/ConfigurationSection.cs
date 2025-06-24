using SC.Abstraction;
using System;

namespace SC;

public class ConfigurationSection(string prefix, IConfiguration configuration) : ConfigurationBase(configuration.Name + configuration.Options.Separator + prefix, configuration.Options), IConfigurationSection
{
    public string Prefix => prefix;

    public override bool HasSection(string prefix) => configuration.HasSection(GetPath(prefix));

    public override bool HasValue(string fullPath) => configuration.HasValue(GetPath(fullPath));

    public override string GetValue(string fullPath) => configuration.GetValue(GetPath(fullPath));

    public override void SetValue(string fullPath, string value) => configuration.SetValue(GetPath(fullPath), value);

    public string GetPath(string pathPart) => prefix + Options.Separator + pathPart;

    public override IConfiguration Clone() => new ConfigurationSection(prefix, configuration);

    IConfiguration IConfiguration.Clone() => Clone();

    object ICloneable.Clone() => Clone();
}