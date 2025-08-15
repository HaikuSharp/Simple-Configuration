using SC.Abstraction;

namespace SC.SR.Extensions;

/// <summary>
/// Provides extension methods for converting configuration options to getters, setters and properties.
/// </summary>
public static class ConfigurationOptionExtensions
{
    /// <summary>
    /// Converts a read-only configuration option to a getter.
    /// </summary>
    /// <typeparam name="T">The type of the option value.</typeparam>
    /// <param name="option">The configuration option.</param>
    /// <returns>The getter instance.</returns>
    public static OptionGetter<T> AsGetter<T>(IReadOnlyConfigurationOption<T> option) => new(option);

    /// <summary>
    /// Converts a configuration option to a setter.
    /// </summary>
    /// <typeparam name="T">The type of the option value.</typeparam>
    /// <param name="option">The configuration option.</param>
    /// <returns>The setter instance.</returns>
    public static OptionSetter<T> AsSetter<T>(IConfigurationOption<T> option) => new(option);

    /// <summary>
    /// Converts a configuration option to a property.
    /// </summary>
    /// <typeparam name="T">The type of the option value.</typeparam>
    /// <param name="option">The configuration option.</param>
    /// <returns>The property instance.</returns>
    public static OptionProperty<T> AsProperty<T>(IConfigurationOption<T> option) => new(option);
}