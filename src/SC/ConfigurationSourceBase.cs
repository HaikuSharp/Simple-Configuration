using SC.Abstraction;

namespace SC;

/// <summary>
/// Base class for configuration sources.
/// </summary>
public abstract class ConfigurationSourceBase(string name) : IConfigurationSource
{
    /// <inheritdoc/>
    public IConfigurationRoot CreateConfiguration(IConfigurationSettings settings) => new Configuration(name, GetValueSource(settings), settings);

    /// <summary>
    /// Gets the value source for the configuration.
    /// </summary>
    /// <param name="settings">The configuration settings.</param>
    /// <returns>The configuration value source.</returns>
    protected abstract IConfigurationValueSource GetValueSource(IConfigurationSettings settings);
}
