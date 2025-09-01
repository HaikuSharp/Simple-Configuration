using SC.Abstraction;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SC.Extensions;

/// <summary>
/// Provides extension methods for <see cref="IConfiguration"/> and <see cref="IReadOnlyConfiguration"/>.
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

    /// <summary>
    /// Retrieves the names of all configuration options at the root level of the configuration.
    /// This method provides a convenient way to discover all top-level configuration options
    /// without specifying a path.
    /// </summary>
    /// <param name="configuration">The configuration to retrieve option names from.</param>
    /// <returns>
    /// An <see cref="IEnumerable{String}"/> containing the names of all configuration options
    /// at the root level. Returns an empty collection if no options are found.
    /// </returns>
    public static IEnumerable<string> GetOptionsNames(this IConfiguration configuration) => configuration.GetOptionsNames(null);

    /// <summary>
    /// Gets an existing configuration option or adds a new one with the specified default value if it doesn't exist.
    /// </summary>
    /// <typeparam name="T">The type of the option value.</typeparam>
    /// <param name="configuration">The configuration to get or add the option to.</param>
    /// <param name="path">The path of the option.</param>
    /// <param name="defaultValueOption">The default option to add if the option doesn't exist.</param>
    /// <returns>The existing option if found; otherwise, the newly added option.</returns>
    public static IConfigurationOption<T> GetOrAddOption<T>(this IConfiguration configuration, string path, IConfigurationOption<T> defaultValueOption) => configuration.TryGetOption<T>(path, out var option) ? option : configuration.AddOption(path, defaultValueOption.Value);

    /// <summary>
    /// Gets an existing configuration option or adds a new one using a factory function to provide the default value if it doesn't exist.
    /// </summary>
    /// <typeparam name="T">The type of the option value.</typeparam>
    /// <param name="configuration">The configuration to get or add the option to.</param>
    /// <param name="path">The path of the option.</param>
    /// <param name="defaultValueFunc">The factory function that provides the default value to add if the option doesn't exist.</param>
    /// <returns>The existing option if found; otherwise, the newly added option.</returns>
    public static IConfigurationOption<T> GetOrAddOption<T>(this IConfiguration configuration, string path, Func<T> defaultValueFunc) => configuration.TryGetOption<T>(path, out var option) ? option : configuration.AddOption(path, defaultValueFunc());

    /// <summary>
    /// Gets an existing configuration option or adds a new one with the specified default value if it doesn't exist.
    /// </summary>
    /// <typeparam name="T">The type of the option value.</typeparam>
    /// <param name="configuration">The configuration to get or add the option to.</param>
    /// <param name="path">The path of the option.</param>
    /// <param name="defaultValue">The default value to add if the option doesn't exist.</param>
    /// <returns>The existing option if found; otherwise, the newly added option.</returns>
    public static IConfigurationOption<T> GetOrAddOption<T>(this IConfiguration configuration, string path, T defaultValue) => configuration.TryGetOption<T>(path, out var option) ? option : configuration.AddOption(path, defaultValue);

    /// <summary>
    /// Attempts to get a configuration option with the specified path and type.
    /// </summary>
    /// <typeparam name="T">The type of the option value.</typeparam>
    /// <param name="configuration">The configuration to get the option from.</param>
    /// <param name="path">The path of the option.</param>
    /// <param name="option">When this method returns, contains the option if found; otherwise, null.</param>
    /// <returns>true if the option was found; otherwise, false.</returns>
    public static bool TryGetOption<T>(this IConfiguration configuration, string path, out IConfigurationOption<T> option)
    {
        option = configuration.GetOption<T>(path);
        return option is not null;
    }

    /// <summary>
    /// Attempts to get a read-only configuration option with the specified path and type.
    /// </summary>
    /// <inheritdoc cref="TryGetOption{T}(IConfiguration, string, out IConfigurationOption{T})"/>
    public static bool TryGetOption<T>(this IReadOnlyConfiguration configuration, string path, out IReadOnlyConfigurationOption<T> option)
    {
        option = configuration.GetOption<T>(path);
        return option is not null;
    }

    /// <summary>
    /// Attempts to get a value from the configuration with the specified path and type.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="configuration">The configuration to get the value from.</param>
    /// <param name="path">The path of the value.</param>
    /// <param name="value">When this method returns, contains the value if found; otherwise, default.</param>
    /// <returns>true if the value was found; otherwise, false.</returns>
    public static bool TryGetValue<T>(this IReadOnlyConfiguration configuration, string path, out T value)
    {
        if(configuration.TryGetOption<T>(path, out var option))
        {
            value = option.Value;
            return true;
        }

        value = default;
        return false;
    }

    /// <summary>
    /// Gets a value from the configuration with the specified path and type.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="configuration">The configuration to get the value from.</param>
    /// <param name="path">The path of the value.</param>
    /// <returns>The value if found; otherwise, default.</returns>
    public static T GetValue<T>(this IReadOnlyConfiguration configuration, string path) => configuration.TryGetOption<T>(path, out var option) ? option.Value : default;

    /// <summary>
    /// Sets a value in the configuration at the specified path.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="configuration">The configuration to set the value in.</param>
    /// <param name="path">The path where to set the value.</param>
    /// <param name="value">The value to set.</param>
    public static void SetValue<T>(this IConfiguration configuration, string path, T value)
    {
        if(!configuration.TryGetOption<T>(path, out var option)) return;
        option.Value = value;
    }

    /// <summary>
    /// Saves the configuration to its default path.
    /// </summary>
    /// <param name="configuration">The configuration to save.</param>
    public static void Save(this IConfiguration configuration) => configuration.Save(null);

    /// <summary>
    /// Loads the configuration from its default path.
    /// </summary>
    /// <param name="configuration">The configuration to load.</param>
    public static void Load(this IConfiguration configuration) => configuration.Load(null);

    /// <summary>
    /// Asynchronously saves the configuration to its default path.
    /// </summary>
    /// <param name="configuration">The configuration to save.</param>
    public static async Task SaveAsync(this IConfiguration configuration) => await configuration.SaveAsync(null);

    /// <summary>
    /// Asynchronously loads the configuration from its default path.
    /// </summary>
    /// <param name="configuration">The configuration to load.</param>
    public static async Task LoadAsync(this IConfiguration configuration) => await configuration.LoadAsync(null);
}