using SC.Abstraction;
using SC.Extensions;
using SR.Abstraction;

namespace SC.SR.Extensions;

/// <summary>
/// Provides extension methods for working with configuration options as getters, setters and properties.
/// </summary>
public static class ConfigurationExtensions
{
    /// <summary>
    /// Gets an existing configuration option or adds a new one with the default value from the getter.
    /// </summary>
    /// <typeparam name="T">The type of the option value.</typeparam>
    /// <param name="configuration">The configuration instance.</param>
    /// <param name="path">The path of the option.</param>
    /// <param name="defaultValueGetter">The getter that provides the default value if the option doesn't exist.</param>
    /// <returns>The existing or newly created configuration option.</returns>
    public static IConfigurationOption<T> GetOrAddOption<T>(this IConfiguration configuration, string path, IGetter<T> defaultValueGetter) => configuration.TryGetOption<T>(path, out var option) ? option : configuration.AddOption(path, defaultValueGetter.Get());

    /// <summary>
    /// Attempts to get a value getter for the specified configuration option.
    /// </summary>
    /// <typeparam name="T">The type of the option value.</typeparam>
    /// <param name="configuration">The configuration instance.</param>
    /// <param name="path">The path of the option.</param>
    /// <param name="getter">When this method returns, contains the getter if found; otherwise, null.</param>
    /// <returns>true if the getter was created; otherwise, false.</returns>
    public static bool TryGetValueGetter<T>(this IReadOnlyConfiguration configuration, string path, out OptionGetter<T> getter) => (getter = configuration.GetValueGetter<T>(path)) is not null;

    /// <summary>
    /// Attempts to get a value setter for the specified configuration option.
    /// </summary>
    /// <typeparam name="T">The type of the option value.</typeparam>
    /// <param name="configuration">The configuration instance.</param>
    /// <param name="path">The path of the option.</param>
    /// <param name="setter">When this method returns, contains the setter if found; otherwise, null.</param>
    /// <returns>true if the setter was created; otherwise, false.</returns>
    public static bool TryGetValueSetter<T>(this IConfiguration configuration, string path, out OptionSetter<T> setter) => (setter = configuration.GetValueSetter<T>(path)) is not null;

    /// <summary>
    /// Attempts to get a value property for the specified configuration option.
    /// </summary>
    /// <typeparam name="T">The type of the option value.</typeparam>
    /// <param name="configuration">The configuration instance.</param>
    /// <param name="path">The path of the option.</param>
    /// <param name="property">When this method returns, contains the property if found; otherwise, null.</param>
    /// <returns>true if the property was created; otherwise, false.</returns>
    public static bool TryGetValueProperty<T>(this IConfiguration configuration, string path, out OptionProperty<T> property) => (property = configuration.GetValueProperty<T>(path)) is not null;

    /// <summary>
    /// Gets a value getter for the specified configuration option.
    /// </summary>
    /// <typeparam name="T">The type of the option value.</typeparam>
    /// <param name="configuration">The configuration instance.</param>
    /// <param name="path">The path of the option.</param>
    /// <returns>The getter instance or null if option not found.</returns>
    public static OptionGetter<T> GetValueGetter<T>(this IReadOnlyConfiguration configuration, string path) => configuration.TryGetOption<T>(path, out var option) ? new(option) : null;

    /// <summary>
    /// Gets a value setter for the specified configuration option.
    /// </summary>
    /// <typeparam name="T">The type of the option value.</typeparam>
    /// <param name="configuration">The configuration instance.</param>
    /// <param name="path">The path of the option.</param>
    /// <returns>The setter instance or null if option not found.</returns>
    public static OptionSetter<T> GetValueSetter<T>(this IConfiguration configuration, string path) => configuration.TryGetOption<T>(path, out var option) ? new(option) : null;

    /// <summary>
    /// Gets a value property for the specified configuration option.
    /// </summary>
    /// <typeparam name="T">The type of the option value.</typeparam>
    /// <param name="configuration">The configuration instance.</param>
    /// <param name="path">The path of the option.</param>
    /// <returns>The property instance or null if option not found.</returns>
    public static OptionProperty<T> GetValueProperty<T>(this IConfiguration configuration, string path) => configuration.TryGetOption<T>(path, out var option) ? new(option) : null;
}