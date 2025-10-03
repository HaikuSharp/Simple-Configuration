namespace SC.Abstraction;

/// <summary>
/// Represents a handler for configuration update events.
/// </summary>
/// <param name="path">The path of the configuration that was updated.</param>
public delegate void ConfigurationUpdateHandler(string path);

/// <summary>
/// Represents the root configuration that provides events for configuration lifecycle.
/// </summary>
public interface IConfigurationRoot : IConfiguration
{
    /// <summary>
    /// Occurs when the configuration is loaded from a persistent store.
    /// </summary>
    event ConfigurationUpdateHandler OnLoaded;

    /// <summary>
    /// Occurs when the configuration is saved to a persistent store.
    /// </summary>
    event ConfigurationUpdateHandler OnSaved;
}