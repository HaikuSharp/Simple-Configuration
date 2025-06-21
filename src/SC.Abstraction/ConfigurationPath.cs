using System;
using System.Collections;
using System.Collections.Generic;
#if NETFRAMEWORK
using System.Linq;
#endif

namespace SC.Abstraction;

public readonly struct ConfigurationPath(string path) : IEquatable<ConfigurationPath>, IEnumerable<string>
{
    public const char PATH_SEPARATOR = ':';
    private readonly string m_RootPath = path;

    public static ConfigurationPath Empty => new(string.Empty);

    public bool IsEmpty => string.IsNullOrWhiteSpace(m_RootPath);

    public bool IsName => !IsEmpty && !m_RootPath.Contains(PATH_SEPARATOR);

    public bool Equals(ConfigurationPath other) => string.Equals(m_RootPath, other.m_RootPath, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object obj) => obj is ConfigurationPath cfgPath && Equals(cfgPath);

    public override int GetHashCode() => m_RootPath.GetHashCode();

    public override string ToString() => m_RootPath;

    public ConfigurationPathEnumerator GetEnumerator() => new(m_RootPath);

    public static ConfigurationPath Combine(ConfigurationPath left, ConfigurationPath rigth) => new(left.m_RootPath + PATH_SEPARATOR + rigth.m_RootPath);

    IEnumerator<string> IEnumerable<string>.GetEnumerator() => GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public static bool operator ==(ConfigurationPath left, ConfigurationPath right) => left.Equals(right);

    public static bool operator !=(ConfigurationPath left, ConfigurationPath right) => !(left == right);
}
