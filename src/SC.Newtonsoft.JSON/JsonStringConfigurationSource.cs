using SC.Abstraction;

namespace SC.Newtonsoft.JSON;

public sealed class JsonStringConfigurationSource(string name, string jsonString) : IConfigurationSource
{
    public IConfiguration Create(IConfigurationOptions options) => new FlattenConfiguration(name, options, JsonHelper.FlattenJson(jsonString, options.Separator));
}