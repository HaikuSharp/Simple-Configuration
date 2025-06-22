using SC.Abstraction;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SC;

public class ConfigurationSection(string name, IDictionary<string, object> values) : ConfigurationSectionBase(name)
{
    private ReadOnlyDictionary<string, object> m_ReadOnlyValues;

    public ConfigurationSection(string name) : this(name, new Dictionary<string, object>()) { }

    public override IConfigurationSection Clone() => new ConfigurationSection(Name, values);

    public override T GetValue<T>(ConfigurationPathEnumerator enumerator)
    {
        if(!enumerator.MoveNext()) return (T)(object)this;
        object currentValue = values[enumerator.Current];
        return currentValue is IConfigurationSection section ? section.GetValue<T>(enumerator) : throw new InvalidOperationException();
    }

    public override void SetValue<T>(ConfigurationPathEnumerator enumerator, T value)
    {
        if(!enumerator.MoveNext()) return;
        string current = enumerator.Current;
        if(values[current] is IConfigurationSection section) section.SetValue(enumerator, value);
        else values[current] = value;
    }

    public override T ToObject<T>() => (m_ReadOnlyValues ??= new ReadOnlyDictionary<string, object>(values)) is T tobj ? tobj : throw new InvalidCastException();

    public override bool TryGetValue<T>(ConfigurationPathEnumerator enumerator, out T value)
    {
        if(!enumerator.MoveNext())
        {
            value = (T)(object)this;
            return true;
        }

        if(!values.TryGetValue(enumerator.Current, out object obj)) goto RET;
        if(obj is IConfigurationSection section) return section.TryGetValue(enumerator, out value);

        if(obj is T tobj)
        {
            value = tobj;
            return true;
        }

    RET:
        value = default;
        return false;
    }

    public override bool TrySetValue<T>(ConfigurationPathEnumerator enumerator, T value)
    {
        if(!enumerator.MoveNext()) return false;
        string current = enumerator.Current;
        if(!values.TryGetValue(current, out object obj)) return false;
        if(obj is not IConfigurationSection section || !section.TrySetValue(enumerator, value)) values[current] = value;
        return true;
    }
}
