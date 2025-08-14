using SC.Abstraction;

namespace SC.Extensions;

public static class ConfigurationRootExtensions
{
    public static bool TryGetConfiguration(this IConfigurationRoot root, string name, out IConfiguration configuration) => (configuration = root.GetConfiguration(name)) is not null;
}
