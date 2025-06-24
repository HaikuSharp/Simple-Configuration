using System;

namespace SC.Abstraction;

public interface IConfiguration : ICloneable
{
    string Name { get; }

    IConfigurationOptions Options { get; }

    bool HasSection(string prefix);

    bool HasValue(string fullPath);

    IConfigurationSection GetSection(string prefix);

    string GetValue(string fullPath);

    void SetValue(string fullPath, string value);

    new IConfiguration Clone();
}
