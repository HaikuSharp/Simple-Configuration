using System.Collections.Generic;

namespace SC.Abstraction;

public interface IReadOnlyConfiguration
{
    string Name { get; }

    IConfigurationSettings Settings { get; }

    IEnumerable<IReadOnlyConfigurationOption> LoadedOptions { get; }

    IReadOnlyConfigurationOption<T> GetOption<T>(string path);
}