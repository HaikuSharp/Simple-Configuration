using System;
using System.Collections;
using System.Collections.Generic;

namespace SC.Abstraction;

public readonly struct ConfigurationPath(string path) : IEquatable<ConfigurationPath>, IEnumerable<string>
{
    private readonly string m_RootPath = path;

    public static string DefaultSeparator => DefaultConfigurationOptions.Default.Separator;

    public static ConfigurationPath Empty => new(string.Empty);

    public bool IsEmpty => string.IsNullOrWhiteSpace(m_RootPath);

    public bool Equals(ConfigurationPath other) => string.Equals(m_RootPath, other.m_RootPath, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object obj) => obj is ConfigurationPath cfgPath && Equals(cfgPath);

    public override int GetHashCode() => m_RootPath.GetHashCode();

    public override string ToString() => m_RootPath;

    public ConfigurationPathEnumerator GetEnumerator() => GetEnumerator(DefaultSeparator);

    public ConfigurationPathEnumerator GetEnumerator(string separator) => new(m_RootPath, separator);

    public bool IsSubPath(ConfigurationPath path) => m_RootPath.Contains(path.m_RootPath);

    public bool IsPrefix(ConfigurationPath path) => m_RootPath.StartsWith(path.m_RootPath);

    public static ConfigurationPath Combine(string left, ConfigurationPath rigth) => Combine(DefaultSeparator, left, rigth);

    public static ConfigurationPath Combine(ConfigurationPath left, string rigth) => Combine(DefaultSeparator, left, rigth);

    public static ConfigurationPath Combine(ConfigurationPath left, ConfigurationPath rigth) => Combine(DefaultSeparator, left, rigth);

    public static ConfigurationPath Combine(string separator, string left, ConfigurationPath rigth) => new(left + separator + rigth.m_RootPath);

    public static ConfigurationPath Combine(string separator, ConfigurationPath left, string rigth) => new(left.m_RootPath + separator + rigth);

    public static ConfigurationPath Combine(string separator, ConfigurationPath left, ConfigurationPath rigth) => new(left.m_RootPath + separator + rigth.m_RootPath);

    IEnumerator<string> IEnumerable<string>.GetEnumerator() => GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public static implicit operator ConfigurationPath(string path) => new(path);

    public static implicit operator string(ConfigurationPath path) => path.m_RootPath;

    public static bool operator ==(ConfigurationPath left, ConfigurationPath right) => left.Equals(right);

    public static bool operator !=(ConfigurationPath left, ConfigurationPath right) => !(left == right);
}
