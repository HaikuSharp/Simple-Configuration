using SC.Abstraction;

namespace SC.Newtonsoft.JSON.Extensions;

public static class ConfigurationBuilderExtensions
{
    public static IConfigurationBuilder AppendJsonFile(this IConfigurationBuilder builder, string name, string filePath) => builder.Append(new JsonFileConfigurationSource(name, filePath));
}
