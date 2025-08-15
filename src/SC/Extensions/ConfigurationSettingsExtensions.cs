using SC.Abstraction;

namespace SC.Extensions;

/// <summary>
/// Provides extension methods for <see cref="IConfigurationSettings"/>.
/// </summary>
public static class ConfigurationSettingsExtensions
{
    /// <summary>
    /// Combines two paths using the separator defined in the settings.
    /// </summary>
    /// <param name="settings">The configuration settings.</param>
    /// <param name="left">The left part of the path.</param>
    /// <param name="right">The right part of the path.</param>
    /// <returns>The combined path.</returns>
    public static string CombinePaths(this IConfigurationSettings settings, string left, string right) => string.IsNullOrEmpty(right) ? left : left + settings.Separator + right;
}