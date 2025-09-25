using SC.Abstraction;

namespace SC.Extensions;

/// <summary>
/// Provides extension methods for <see cref="IConfiguration"/>.
/// </summary>
public static class ConfigurationExtensions
{
    /// <summary>
    /// Gets a configuration section with the specified path.
    /// </summary>
    /// <param name="configuration">The configuration to get the section from.</param>
    /// <param name="path">The path of the section.</param>
    /// <returns>The configuration section.</returns>
    public static IConfigurationSection GetSection(this IConfiguration configuration, string path) => new ConfigurationSection(configuration, path);

    public static TOption AddValueOption<TOption, TValue>(this IConfiguration configuration, string path, TValue value) where TOption : class, IValueConfigurationOption<TValue>, new()
    {
        var option = configuration.AddOption<TOption>(path);
        option.Value = value;
        return option;
    }

    public static TOption GetOrAddOption<TOption>(this IConfiguration configuration, string path) where TOption : class, IConfigurationOption, new() => configuration.GetOption<TOption>(path) ?? configuration.AddOption<TOption>(path);

    public static bool TryGetOption<TOption>(this IConfiguration configuration, string path, out TOption option) where TOption : class, IConfigurationOption, new() => (option = configuration.GetOption<TOption>(path)) is not null;

    /// <summary>
    /// Saves the configuration in loaded source with default path.
    /// </summary>
    /// <param name="configuration">The configuration to save.</param>
    public static void Save(this IConfiguration configuration) => configuration.Save(null);

    /// <summary>
    /// Loads the configuration from loaded source with default path.
    /// </summary>
    /// <param name="configuration">The configuration to load.</param>
    public static void Load(this IConfiguration configuration) => configuration.Load(null);

    /// <summary>
    /// Saves the configuration with default path.
    /// </summary>
    /// <param name="configuration">The configuration to save.</param>
    /// <param name="source">The value source.</param>
    public static void Save(this IConfiguration configuration, IConfigurationValueSource source) => configuration.Save(null, source);

    /// <summary>
    /// Loads the configuration with default path.
    /// </summary>
    /// <param name="configuration">The configuration to load.</param>
    /// <param name="source">The value source.</param>
    public static void Load(this IConfiguration configuration, IConfigurationValueSource source) => configuration.Load(null, source);
}