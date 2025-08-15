namespace SC.Abstraction;

/// <summary>
/// Provides a mechanism for building configuration hierarchies.
/// </summary>
public interface IConfigurationBuilder
{
    /// <summary>
    /// Builds a configuration root with the specified name and settings.
    /// </summary>
    /// <param name="name">The name of the configuration root.</param>
    /// <param name="settings">The settings to use for the configuration.</param>
    /// <returns>The created configuration root.</returns>
    IConfigurationRoot Build(string name, IConfigurationSettings settings);

    /// <summary>
    /// Appends a configuration source to the builder.
    /// </summary>
    /// <param name="source">The configuration source to append.</param>
    /// <returns>The configuration builder for chaining.</returns>
    IConfigurationBuilder Append(IConfigurationSource source);
}