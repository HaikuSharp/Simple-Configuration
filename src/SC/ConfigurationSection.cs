using SC.Abstraction;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SC;

public class ConfigurationSection(string name, IDictionary<string, object> values) : ConfigurationSectionBase(name)
{
    private ReadOnlyDictionary<string, object> m_ReadOnlyValues;

    public ConfigurationSection(string name) : this(name, new Dictionary<string, object>()) { }

    public override T GetValue<T>(ConfigurationPathEnumerator enumerator)
    {
        if(!enumerator.MoveNext()) return (T)(object)this;
        object currentValue = values[enumerator.Current];
        return currentValue is IConfigurationSection section ? section.GetValue<T>(enumerator) : (T)currentValue;
    }

    public override void SetValue<T>(ConfigurationPathEnumerator enumerator, T value)
    {
        if(!enumerator.MoveNext()) return;
        string current = enumerator.Current;
        if(values[current] is IConfigurationSection section) section.SetValue(enumerator, value);
        else values[current] = value;
    }

    public override T ToObject<T>() => (m_ReadOnlyValues ??= new ReadOnlyDictionary<string, object>(values)) is T tobj ? tobj : throw new InvalidCastException();
}
