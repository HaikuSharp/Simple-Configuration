using SC.Abstraction;
using System;
using System.Collections.Generic;

namespace SC;

public abstract class ConfigurationBase(string name, IConfigurationOptions options) : IConfiguration
{
    private readonly Dictionary<string, IConfigurationSection> m_SectionsCache = [];

    public ConfigurationBase(string name) : this(name, DefaultConfigurationOptions.Default) { }

    public string Name => name;

    public IConfigurationOptions Options => options;

    public abstract bool HasSection(string prefix);

    public abstract bool HasValue(string fullPath);

    public IConfigurationSection GetSection(string prefix)
    {
        if(m_SectionsCache.TryGetValue(prefix, out var section)) return section;

        if(HasSection(prefix))
        {
            section = new ConfigurationSection(prefix, this);
            m_SectionsCache.Add(prefix, section);
            return section;
        }

        return null;
    }

    public abstract string GetValue(string fullPath);

    public abstract void SetValue(string fullPath, string value);

    public abstract IConfiguration Clone();

    object ICloneable.Clone() => Clone();
}
