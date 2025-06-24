using Newtonsoft.Json.Linq;
using SC.Abstraction;
using System.Collections.Generic;
using System.IO;

namespace SC.Newtonsoft.JSON;

public class JsonFileConfigurationSource(string fileName) : IConfigurationSource
{
    public IConfiguration Create(IConfigurationOptions options) => new Configuration(Path.GetFileNameWithoutExtension(fileName), options, FlattenJson(LoadJsonFile(), options.Separator));

    private static IDictionary<string, string> FlattenJson(JToken token, string separator)
    {
        IDictionary<string, string> json = new Dictionary<string, string>();
        FlattenToken(token, json, string.Empty, separator);
        return json;
    }

    private static void FlattenToken(JToken token, IDictionary<string, string> result, string prefix, string separator)
    {
        string MergePrefix<T>(T obj) => string.IsNullOrEmpty(prefix) ? obj.ToString() : $"{prefix}{separator}{obj}";

        switch(token.Type)
        {
            case JTokenType.Object:
            foreach(var property in token.Children<JProperty>()) FlattenToken(property.Value, result, MergePrefix(property.Name), separator);
            break;
            case JTokenType.Array:
            int index = 0;
            foreach(var value in token.Children())
            {
                FlattenToken(value, result, MergePrefix(index), separator);
                index++;
            }
            break;
            default:
            result.Add(prefix, token.ToString());
            break;
        }
    }

    private JToken LoadJsonFile() => File.Exists(fileName) ? JToken.Parse(File.ReadAllText(fileName)) : throw new FileNotFoundException(null, fileName);
}
