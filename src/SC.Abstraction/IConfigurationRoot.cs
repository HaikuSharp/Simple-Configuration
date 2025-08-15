using System.Collections.Generic;

namespace SC.Abstraction;

/// <summary>
/// Represents the root of a configuration hierarchy that can contain multiple configurations.
/// </summary>
public interface IConfigurationRoot : IConfiguration
{
    /// <summary>
    /// Gets all loaded configurations.
    /// </summary>
    IEnumerable<IConfiguration> LoadedConfigurations { get; }

    /// <summary>
    /// Determines whether the root contains a configuration with the specified name.
    /// </summary>
    /// <param name="name">The name of the configuration to locate.</param>
    /// <returns>
    /// <c>true</c> if the root contains a configuration with the specified name; otherwise, <c>false</c>.
    /// </returns>
    bool HasConfiguration(string name);

    /// <summary>
    /// Gets the configuration with the specified name.
    /// </summary>
    /// <param name="name">The name of the configuration to get.</param>
    /// <returns>The configuration.</returns>
    IConfiguration GetConfiguration(string name);

    /// <summary>
    /// Adds a configuration to the root.
    /// </summary>
    /// <param name="configuration">The configuration to add.</param>
    void AddConfiguration(IConfiguration configuration);

    /// <summary>
    /// Removes the configuration with the specified name.
    /// </summary>
    /// <param name="name">The name of the configuration to remove.</param>
    void RemoveConfiguration(string name);
}