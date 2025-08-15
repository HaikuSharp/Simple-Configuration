using SC.Abstraction;

namespace SC.Extensions;

/// <summary>
/// Provides extension methods for <see cref="IConfigurationRoot"/>.
/// </summary>
public static class ConfigurationRootExtensions
{
    /// <summary>
    /// Attempts to get a configuration with the specified name.
    /// </summary>
    /// <param name="root">The configuration root to get the configuration from.</param>
    /// <param name="name">The name of the configuration.</param>
    /// <param name="configuration">When this method returns, contains the configuration if found; otherwise, null.</param>
    /// <returns>true if the configuration was found; otherwise, false.</returns>
    public static bool TryGetConfiguration(this IConfigurationRoot root, string name, out IConfiguration configuration) => (configuration = root.GetConfiguration(name)) is not null;
}