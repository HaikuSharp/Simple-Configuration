using Newtonsoft.Json.Linq;
using SC.Abstraction;

namespace SC.Newtonsoft.JSON.Extensions;

public static class ConfigurationBuilderExtensions
{
    public static IConfigurationBuilder AppendJsonString(this IConfigurationBuilder builder, string name, string jsonString) => builder.AppendJson(name, JToken.Parse(jsonString));

    public static IConfigurationBuilder AppendJson(this IConfigurationBuilder builder, string name, JToken token) => builder.Append(new JsonConfigurationSource(name, token));
}
