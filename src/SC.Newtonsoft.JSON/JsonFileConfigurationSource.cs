using Newtonsoft.Json.Linq;
using SC.Abstraction;
using System.IO;

namespace SC.Newtonsoft.JSON;

public sealed class JsonFileConfigurationSource(string fileName) : IConfigurationSource
{
    public IConfiguration Create(IConfigurationOptions options) => new FlattenConfiguration(Path.GetFileNameWithoutExtension(fileName), options, JsonHelper.FlattenJson(LoadJsonJsonTokenFromFile(), options.Separator));

    private JToken LoadJsonJsonTokenFromFile() => File.Exists(fileName) ? JToken.Parse(File.ReadAllText(fileName)) : throw new FileNotFoundException(null, fileName);
}
