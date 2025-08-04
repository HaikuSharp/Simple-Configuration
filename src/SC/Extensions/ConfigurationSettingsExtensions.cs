using SC.Abstraction;

namespace SC.Extensions;

public static class ConfigurationSettingsExtensions
{
    public static string CombinePaths(this IConfigurationSettings settings, string left, string right) => left + settings.Separator + right;
}
