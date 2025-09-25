using SC.Abstraction;
using System;

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

    /// <summary>
    /// Gets an existing value configuration option or adds a new one using a factory function if it doesn't exist.
    /// </summary>
    /// <typeparam name="TOption">The type of the value configuration option.</typeparam>
    /// <typeparam name="TValue">The type of the value held by the option.</typeparam>
    /// <param name="configuration">The configuration to get or add the option from.</param>
    /// <param name="path">The path of the option.</param>
    /// <param name="constructor">The factory function to create the value if the option doesn't exist.</param>
    /// <returns>The existing or newly created configuration option.</returns>
    public static TOption GetOrAddValueOption<TOption, TValue>(this IConfiguration configuration, string path, Func<TValue> constructor) where TOption : class, IValueConfigurationOption<TValue>, new() => configuration.GetOption<TOption>(path) ?? configuration.AddValueOption<TOption, TValue>(path, constructor());

    /// <summary>
    /// Gets an existing value configuration option or adds a new one with the specified value if it doesn't exist.
    /// </summary>
    /// <typeparam name="TOption">The type of the value configuration option.</typeparam>
    /// <typeparam name="TValue">The type of the value held by the option.</typeparam>
    /// <param name="configuration">The configuration to get or add the option from.</param>
    /// <param name="path">The path of the option.</param>
    /// <param name="value">The value to set if the option doesn't exist.</param>
    /// <returns>The existing or newly created configuration option.</returns>
    public static TOption GetOrAddValueOption<TOption, TValue>(this IConfiguration configuration, string path, TValue value) where TOption : class, IValueConfigurationOption<TValue>, new() => configuration.GetOption<TOption>(path) ?? configuration.AddValueOption<TOption, TValue>(path, value);

    /// <summary>
    /// Adds a new value configuration option with the specified path and value.
    /// </summary>
    /// <typeparam name="TOption">The type of the value configuration option.</typeparam>
    /// <typeparam name="TValue">The type of the value held by the option.</typeparam>
    /// <param name="configuration">The configuration to add the option to.</param>
    /// <param name="path">The path of the option to add.</param>
    /// <param name="value">The value to set for the new option.</param>
    /// <returns>The created configuration option.</returns>
    public static TOption AddValueOption<TOption, TValue>(this IConfiguration configuration, string path, TValue value) where TOption : class, IValueConfigurationOption<TValue>, new()
    {
        var option = configuration.AddOption<TOption>(path);
        option.Value = value;
        return option;
    }

    /// <summary>
    /// Gets an existing configuration option or adds a new one if it doesn't exist.
    /// </summary>
    /// <typeparam name="TOption">The type of the configuration option.</typeparam>
    /// <param name="configuration">The configuration to get or add the option from.</param>
    /// <param name="path">The path of the option.</param>
    /// <returns>The existing or newly created configuration option.</returns>
    public static TOption GetOrAddOption<TOption>(this IConfiguration configuration, string path) where TOption : class, IConfigurationOption, new() => configuration.GetOption<TOption>(path) ?? configuration.AddOption<TOption>(path);

    /// <summary>
    /// Attempts to get a configuration option with the specified path.
    /// </summary>
    /// <typeparam name="TOption">The type of the configuration option.</typeparam>
    /// <param name="configuration">The configuration to get the option from.</param>
    /// <param name="path">The path of the option.</param>
    /// <param name="option">When this method returns, contains the configuration option if found; otherwise, null.</param>
    /// <returns>true if the configuration option was found; otherwise, false.</returns>
    public static bool TryGetOption<TOption>(this IConfiguration configuration, string path, out TOption option) where TOption : class, IConfigurationOption, new() => (option = configuration.GetOption<TOption>(path)) is not null;

    /// <summary>
    /// Saves the configuration using the previously loaded source with the default path.
    /// </summary>
    /// <param name="configuration">The configuration to save.</param>
    public static void Save(this IConfiguration configuration) => configuration.Save(null);

    /// <summary>
    /// Loads the configuration from the previously used source with the default path.
    /// </summary>
    /// <param name="configuration">The configuration to load.</param>
    public static void Load(this IConfiguration configuration) => configuration.Load(null);

    /// <summary>
    /// Saves the configuration with the specified source using the default path.
    /// </summary>
    /// <param name="configuration">The configuration to save.</param>
    /// <param name="source">The value source to save to.</param>
    public static void Save(this IConfiguration configuration, IConfigurationValueSource source) => configuration.Save(null, source);

    /// <summary>
    /// Loads the configuration from the specified source using the default path.
    /// </summary>
    /// <param name="configuration">The configuration to load.</param>
    /// <param name="source">The value source to load from.</param>
    public static void Load(this IConfiguration configuration, IConfigurationValueSource source) => configuration.Load(null, source);
}