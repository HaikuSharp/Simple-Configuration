using SC.Abstraction;

namespace SC.Extensions;

public static class ConfigurationSettingsExtensions
{
    public static string CombinePaths(this IConfigurationSettings settings, string left, string right) => string.IsNullOrEmpty(right) ? left : left + settings.Separator + right;
}
