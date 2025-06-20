#if NETFRAMEWORK
using System.Linq;
#endif

namespace SC.Abstraction;

public abstract class ConfigurationSectionBase(string name) : IConfigurationSection
{
    public string Name => name;

    public T GetValue<T>(ConfigurationPath path) => GetValue<T>(path.GetEnumerator());

    public abstract T GetValue<T>(ConfigurationPathEnumerator enumerator);

    public void SetValue<T>(ConfigurationPath path, T value) => SetValue(path.GetEnumerator(), value);

    public abstract void SetValue<T>(ConfigurationPathEnumerator enumerator, T value);

    public abstract T ToObject<T>();
}