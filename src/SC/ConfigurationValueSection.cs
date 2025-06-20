using SC.Abstraction;
using System;

namespace SC;

public class ConfigurationValueSection<TValue>(string name, TValue value) : ConfigurationSectionBase(name)
{
    public const string VALUE_NAME = "value";

    public T GetValue<T>() => GetValue() is T tvalue ? tvalue : throw new InvalidCastException();

    public TValue GetValue() => value;

    public void SetValue<T>(T nvalue)
    {
        if(nvalue is not TValue tvalue) return;
        SetValue(tvalue);
    }

    public void SetValue(TValue nvalue) => value = nvalue;

    public override T ToObject<T>() => GetValue() is T tobj ? tobj : throw new InvalidCastException();

    public override T GetValue<T>(ConfigurationPathEnumerator enumerator)
    {
        if(!enumerator.MoveNext()) return (T)(object)this;
        string current = enumerator.Current;
        return current.Equals(VALUE_NAME, StringComparison.OrdinalIgnoreCase) ? GetValue<T>() : throw new InvalidOperationException();
    }

    public override void SetValue<T>(ConfigurationPathEnumerator enumerator, T value)
    {
        if(!enumerator.MoveNext()) return;
        string current = enumerator.Current;
        if(current.Equals(VALUE_NAME, StringComparison.OrdinalIgnoreCase)) SetValue(value);
    }

    public static explicit operator TValue(ConfigurationValueSection<TValue> section) => section.GetValue();
}
