using System.Collections.Generic;

namespace SC.Abstraction;

/// <summary>
/// Represents a read-only configuration with options that can be accessed but not modified.
/// </summary>
public interface IReadOnlyConfiguration
{
    /// <summary>
    /// Gets the name of the configuration.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the settings used by the configuration.
    /// </summary>
    IConfigurationSettings Settings { get; }

    /// <summary>
    /// Gets all loaded configuration options.
    /// </summary>
    IEnumerable<IReadOnlyConfigurationOption> LoadedOptions { get; }

    /// <summary>
    /// Gets the read-only configuration option with the specified path and type.
    /// </summary>
    /// <typeparam name="T">The type of the option value.</typeparam>
    /// <param name="path">The path of the option to get.</param>
    /// <returns>The read-only configuration option.</returns>
    IReadOnlyConfigurationOption<T> GetOption<T>(string path);
}