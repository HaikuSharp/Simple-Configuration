using System.Collections.Generic;

namespace SC.Abstraction;

public interface IConfiguration
{
    string Name { get; }

    IConfigurationSettings Settings { get; }

    bool HasOption(string path);

    IConfigurationOption<T> GetOption<T>(string path);

    IConfigurationOption<T> AddOption<T>(string path, T value);
}
