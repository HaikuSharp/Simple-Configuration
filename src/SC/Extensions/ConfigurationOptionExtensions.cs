using SC.Abstraction;

namespace SC.Extensions;

public static class ConfigurationOptionExtensions
{
    public static bool IsDirty(this IConfigurationOption option) => option.Version is not 0;
}