using System;

namespace SC.Abstraction;

public interface IConfigurationSection : IToObjectConvertable, ICloneable
{
    string Name { get; }

    bool Has(ConfigurationPath path);

    bool Has(ConfigurationPathEnumerator enumerator);

    T GetValue<T>(ConfigurationPath path);

    void SetValue<T>(ConfigurationPath path, T value);

    T GetValue<T>(ConfigurationPathEnumerator enumerator);

    void SetValue<T>(ConfigurationPathEnumerator enumerator, T value);

    bool TryGetValue<T>(ConfigurationPath path, out T value);

    bool TrySetValue<T>(ConfigurationPath path, T value);

    bool TryGetValue<T>(ConfigurationPathEnumerator enumerator, out T value);

    bool TrySetValue<T>(ConfigurationPathEnumerator enumerator, T value);

    new IConfigurationSection Clone();
}
