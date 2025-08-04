using SC.Abstraction;

namespace SC.Newtonsoft.JSON.Extensions;

public static class ConfigurationBuilderExtensions
{
    public static IConfigurationBuilder AppendJsonString(this IConfigurationBuilder builder, string name, string jsonString) => builder.Append(new JsonStringConfigurationSource(name, jsonString));
}
