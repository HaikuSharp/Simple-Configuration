using Newtonsoft.Json.Linq;
using SC.Abstraction;

namespace SC.Newtonsoft.JSON;

public class JsonStringConfigurationSource(string name, string jsonString) : IConfigurationSource
{
    public IConfiguration GetConfiguration(IConfigurationSettings settings) => new JsonConfiguration(name, JToken.Parse(jsonString), settings);
}
