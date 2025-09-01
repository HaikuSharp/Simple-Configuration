namespace SC.Abstraction;

/// <summary>
/// Represents a source of configuration data.
/// </summary>
public interface IConfigurationSource
{
    /// <summary>
    /// Creates a configuration using the specified settings.
    /// </summary>
    /// <param name="settings">The settings to use for the configuration.</param>
    /// <returns>The created configuration.</returns>
    IConfigurationRoot CreateConfiguration(IConfigurationSettings settings);
}