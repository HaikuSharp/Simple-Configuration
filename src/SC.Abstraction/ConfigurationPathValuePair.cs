using System;
using System.Collections.Generic;

namespace SC.Abstraction;

public readonly struct ConfigurationPathValuePair(ConfigurationPath path, ConfigurationValue value) : IEquatable<ConfigurationPathValuePair>
{
    public ConfigurationPath Path => path;

    public ConfigurationValue Value => value;

    public bool Equals(ConfigurationPathValuePair other) => path == other.Path;

    public override bool Equals(object obj) => obj is ConfigurationPathValuePair pair && Equals(pair);

    public override int GetHashCode() => path.GetHashCode();

    public override string ToString() => $"({Path}: {value})";

    public void Destruct(out ConfigurationPath path, out ConfigurationValue value)
    {
        path = Path;
        value = Value;
    }

    public static ConfigurationPathValuePair FromKvp(KeyValuePair<ConfigurationPath, ConfigurationValue> kvp) => new(kvp.Key, kvp.Value);

    public static bool operator ==(ConfigurationPathValuePair left, ConfigurationPathValuePair right) => left.Equals(right);

    public static bool operator !=(ConfigurationPathValuePair left, ConfigurationPathValuePair right) => !(left == right);
}