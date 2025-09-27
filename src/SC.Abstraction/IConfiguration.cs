using System.Collections.Generic;

namespace SC.Abstraction;

/// <summary>
/// Represents a configuration with options that can be loaded, modified and saved.
/// </summary>
public interface IConfiguration
{
    /// <summary>
    /// Gets the settings used by the configuration.
    /// </summary>
    IConfigurationSettings Settings { get; }

    /// <summary>
    /// Gets whether the source has been loaded at least once or not
    /// </summary>
    bool HasLoadedSource { get; }

    /// <summary>
    /// Determines whether the configuration contains an option with the specified path.
    /// </summary>
    /// <param name="path">The path of the option to locate.</param>
    /// <returns>
    /// <c>true</c> if the configuration contains an option with the specified path; otherwise, <c>false</c>.
    /// </returns>
    bool HasOption(string path);

    /// <summary>
    /// Retrieves the names of all configuration options located at the specified path.
    /// This method is useful for discovering available configuration options within
    /// a specific configuration section or hierarchy level.
    /// </summary>
    /// <param name="path">The path to search for configuration options.</param>
    /// <returns>
    /// An <see cref="IEnumerable{String}"/> containing the names of all configuration options
    /// found at the specified path. Returns an empty collection if no options are found.
    /// </returns>
    IEnumerable<string> GetOptionsNames(string path);

    /// <summary>
    /// Gets the configuration option with the specified path and type.
    /// </summary>
    /// <typeparam name="TOption">The type of the option.</typeparam>
    /// <param name="path">The path of the option to get.</param>
    /// <returns>The configuration option.</returns>
    TOption GetOption<TOption>(string path) where TOption : ConfigurationOptionBase, new();

    /// <summary>
    /// Adds a new configuration option with the specified path and value.
    /// </summary>
    /// <typeparam name="TOption">The type of the option.</typeparam>
    /// <param name="path">The path of the option to add.</param>
    /// <returns>The created configuration option.</returns>
    TOption AddOption<TOption>(string path) where TOption : ConfigurationOptionBase, new();

    /// <summary>
    /// Removes the configuration option with the specified path.
    /// </summary>
    /// <param name="path">The path of the option to remove.</param>
    void RemoveOption(string path);

    /// <summary>
    /// Saves the configuration segment with the specified path.
    /// </summary>
    /// <param name="path">The segment configuration path where to save.</param>
    /// <param name="source">The value source.</param>
    void Save(string path, IConfigurationValueSource source);

    /// <summary>
    /// Loads the configuration segment with the specified path.
    /// </summary>
    /// <param name="path">The configuration section path.</param>
    /// <param name="source">The value source.</param>
    void Load(string path, IConfigurationValueSource source);
}
