using SC.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SC;

public abstract class ConfigurationBase(string name, IConfigurationOptions options) : IConfiguration
{
    private readonly Dictionary<ConfigurationPath, IConfigurationSection> m_SectionsCache = [];

    public string Name => name;

    public virtual int ValuesCount => Pairs.Count();

    public abstract IEnumerable<ConfigurationPathValuePair> Pairs { get; }

    public abstract IEnumerable<ConfigurationPath> Paths { get; }

    public abstract IEnumerable<ConfigurationValue> Values { get; }

    public IConfigurationOptions Options => options;

    public ConfigurationValue this[ConfigurationPath path]
    {
        get => GetValue(path);
        set => SetValue(path, value);
    }

    public abstract bool HasSection(ConfigurationPath prefix);

    public abstract bool HasValue(ConfigurationPath fullPath);

    public IConfigurationSection GetSection(ConfigurationPath prefix)
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

    public abstract ConfigurationValue GetValue(ConfigurationPath fullPath);

    public abstract void SetValue(ConfigurationPath fullPath, ConfigurationValue value);

    public abstract IConfiguration Clone();

    object ICloneable.Clone() => Clone();
}
