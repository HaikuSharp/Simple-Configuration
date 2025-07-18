using System;
using System.Collections;
using System.Collections.Generic;

namespace SC.Abstraction;

public readonly struct ConfigurationPath(string path) : IEquatable<ConfigurationPath>, IEnumerable<string>
{
    private readonly string m_SourcePath = path;

    public static string DefaultSeparator => DefaultConfigurationOptions.Default.Separator;

    public static ConfigurationPath Empty => new(string.Empty);

    public bool IsEmpty => string.IsNullOrWhiteSpace(m_SourcePath);

    public bool IsLong() => IsLong(DefaultSeparator);

    public bool IsLong(string separator) => m_SourcePath.Contains(separator);

    public bool Equals(ConfigurationPath other) => string.Equals(m_SourcePath, other.m_SourcePath, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object obj) => obj is ConfigurationPath cfgPath && Equals(cfgPath);

    public override int GetHashCode() => m_SourcePath.GetHashCode();

    public override string ToString() => m_SourcePath;

    public ConfigurationPathEnumerator GetEnumerator() => GetEnumerator(DefaultSeparator);

    public ConfigurationPathEnumerator GetEnumerator(string separator) => new(m_SourcePath, separator);

    public bool IsSubPath(ConfigurationPath path) => m_SourcePath.Contains(path.m_SourcePath);

    public bool IsPrefix(ConfigurationPath path) => m_SourcePath.StartsWith(path.m_SourcePath);

    public static ConfigurationPath Combine(string left, ConfigurationPath rigth) => Combine(DefaultSeparator, left, rigth);

    public static ConfigurationPath Combine(ConfigurationPath left, string rigth) => Combine(DefaultSeparator, left, rigth);

    public static ConfigurationPath Combine(ConfigurationPath left, ConfigurationPath rigth) => Combine(DefaultSeparator, left, rigth);

    public static ConfigurationPath Combine(string left, string rigth) => Combine(DefaultSeparator, left, rigth);

    public static ConfigurationPath Combine(string separator, string left, ConfigurationPath rigth) => Combine(separator, left, rigth.m_SourcePath);

    public static ConfigurationPath Combine(string separator, ConfigurationPath left, string rigth) => Combine(separator, left.m_SourcePath, rigth);

    public static ConfigurationPath Combine(string separator, ConfigurationPath left, ConfigurationPath rigth) => Combine(separator, left.m_SourcePath, rigth.m_SourcePath);

    public static ConfigurationPath Combine(string separator, string left, string rigth) => new(left + separator + rigth);

    IEnumerator<string> IEnumerable<string>.GetEnumerator() => GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public static implicit operator ConfigurationPath(string path) => new(path);

    public static implicit operator string(ConfigurationPath path) => path.m_SourcePath;

    public static bool operator ==(ConfigurationPath left, ConfigurationPath right) => left.Equals(right);

    public static bool operator !=(ConfigurationPath left, ConfigurationPath right) => !(left == right);
}
