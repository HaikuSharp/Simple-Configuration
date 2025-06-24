using SC.Abstraction;

namespace SC.Newtonsoft.JSON;

public static class JsonConfigurationBuilderExtensions
{
    public static IConfigurationBuilder AppendJsonFile(this IConfigurationBuilder builder, string filePath) => builder.AppendSource(new JsonFileConfigurationSource(filePath));
}
