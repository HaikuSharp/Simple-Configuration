using System.Collections.Generic;
using System.Threading.Tasks;

namespace SC.Abstraction;

/// <summary>
/// Represents a mutable configuration with options that can be loaded, modified and saved.
/// </summary>
public interface IConfiguration : IReadOnlyConfiguration
{
    /// <summary>
    /// Gets all loaded configuration options.
    /// </summary>
    new IEnumerable<IConfigurationOption> LoadedOptions { get; }

    /// <summary>
    /// Determines whether the configuration contains an option with the specified path.
    /// </summary>
    /// <param name="path">The path of the option to locate.</param>
    /// <returns>
    /// <c>true</c> if the configuration contains an option with the specified path; otherwise, <c>false</c>.
    /// </returns>
    bool HasOption(string path);

    /// <summary>
    /// Gets the configuration option with the specified path and type.
    /// </summary>
    /// <typeparam name="T">The type of the option value.</typeparam>
    /// <param name="path">The path of the option to get.</param>
    /// <returns>The configuration option.</returns>
    new IConfigurationOption<T> GetOption<T>(string path);

    /// <summary>
    /// Adds a new configuration option with the specified path and value.
    /// </summary>
    /// <typeparam name="T">The type of the option value.</typeparam>
    /// <param name="path">The path of the option to add.</param>
    /// <param name="value">The value of the option.</param>
    /// <returns>The created configuration option.</returns>
    IConfigurationOption<T> AddOption<T>(string path, T value);

    /// <summary>
    /// Removes the configuration option with the specified path.
    /// </summary>
    /// <param name="path">The path of the option to remove.</param>
    void RemoveOption(string path);

    /// <summary>
    /// Saves the configuration segment with the specified path.
    /// </summary>
    /// <param name="path">The segment configuration path where to save.</param>
    void Save(string path);

    /// <summary>
    /// Loads the configuration segment with the specified path.
    /// </summary>
    /// <param name="path">The configuration section path.</param>
    void Load(string path);

    /// <summary>
    /// Asynchronously saves the configuration segment with the specified path.
    /// </summary>
    /// <param name="path">The segment configuration path where to save.</param>
    Task SaveAsync(string path);

    /// <summary>
    /// Asynchronously loads the configuration segment with the specified path.
    /// </summary>
    /// <param name="path">The path from which to load the configuration.</param>
    Task LoadAsync(string path);
}