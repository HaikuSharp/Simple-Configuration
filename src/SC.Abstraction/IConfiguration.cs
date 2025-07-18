using System;
using System.Collections.Generic;

namespace SC.Abstraction;

public interface IConfiguration : ICloneable
{
    string Name { get; }

    int ValuesCount { get; }

    ConfigurationValue this[ConfigurationPath path] { get; set; }

    IEnumerable<ConfigurationPathValuePair> Pairs { get; }

    IEnumerable<ConfigurationPath> Paths { get; }

    IEnumerable<ConfigurationValue> Values { get; }

    IConfigurationOptions Options { get; }

    bool HasSection(ConfigurationPath prefix);

    bool HasValue(ConfigurationPath fullPath);

    IConfigurationSection GetSection(ConfigurationPath prefix);

    ConfigurationValue GetValue(ConfigurationPath fullPath);

    void SetValue(ConfigurationPath fullPath, ConfigurationValue value);

    void Add(ConfigurationPath fullPath, ConfigurationValue value);

    void Remove(ConfigurationPath fullPath);

    new IConfiguration Clone();
}
