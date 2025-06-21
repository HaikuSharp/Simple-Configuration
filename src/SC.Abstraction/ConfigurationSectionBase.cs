#if NETFRAMEWORK
using System.Linq;
#endif

using System;

namespace SC.Abstraction;

public abstract class ConfigurationSectionBase(string name) : IConfigurationSection
{
    public string Name => name;

    public bool Has(ConfigurationPath path) => throw new System.NotImplementedException();
    public bool Has(ConfigurationPathEnumerator enumerator) => throw new System.NotImplementedException();

    public T GetValue<T>(ConfigurationPath path) => GetValue<T>(path.GetEnumerator());

    public abstract T GetValue<T>(ConfigurationPathEnumerator enumerator);

    public void SetValue<T>(ConfigurationPath path, T value) => SetValue(path.GetEnumerator(), value);

    public abstract void SetValue<T>(ConfigurationPathEnumerator enumerator, T value);

    public abstract T ToObject<T>();

    public bool TryGetValue<T>(ConfigurationPath path, out T value) => TryGetValue(path.GetEnumerator(), out value);

    public abstract bool TryGetValue<T>(ConfigurationPathEnumerator enumerator, out T value);

    public bool TrySetValue<T>(ConfigurationPath path, T value) => TrySetValue(path.GetEnumerator(), value);

    public abstract bool TrySetValue<T>(ConfigurationPathEnumerator enumerator, T value);

    public abstract IConfigurationSection Clone();

    object ICloneable.Clone() => Clone();
}