using SC.Abstraction;
using System;

namespace SC;

public abstract class ConfigurationBase(string name, IConfigurationOptions options) : IConfiguration
{
    public ConfigurationBase(string name) : this(name, DefaultConfigurationOptions.Default) { }

    public string Name => name;

    public IConfigurationOptions Options => options;

    public abstract bool HasSection(string prefix);

    public abstract bool HasValue(string fullPath);

    public IConfigurationSection GetSection(string prefix) => HasSection(prefix) ? new ConfigurationSection(prefix, this) : null;

    public abstract string GetValue(string fullPath);

    public abstract void SetValue(string fullPath, string value);

    public abstract IConfiguration Clone();

    object ICloneable.Clone() => Clone();
}
