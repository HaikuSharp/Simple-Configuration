using SC.Abstraction;

namespace SC.Newtonsoft.JSON.Extensions;

public static class JsonConfigurationBuilderExtensions
{
    public static IConfigurationBuilder AppendJsonString(this IConfigurationBuilder builder, string name, string jsonString) => builder.AppendSource(new JsonStringConfigurationSource(name, jsonString));

    public static IConfigurationBuilder AppendJsonFile(this IConfigurationBuilder builder, string filePath) => builder.AppendSource(new JsonFileConfigurationSource(filePath));
}
