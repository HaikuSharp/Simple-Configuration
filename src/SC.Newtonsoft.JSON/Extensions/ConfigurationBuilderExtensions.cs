using SC.Abstraction;

namespace SC.Newtonsoft.JSON.Extensions;

public static class ConfigurationBuilderExtensions
{
    public static IConfigurationBuilder AppendJsonFile(this IConfigurationBuilder builder, string filePath) => builder.Append(new JsonFileConfigurationSource(filePath));
}
