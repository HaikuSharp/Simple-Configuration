using Newtonsoft.Json.Linq;
using SC.Abstraction;
using System.Collections.Generic;

namespace SC.Newtonsoft.JSON;

public class JsonHelper
{
    public static IDictionary<ConfigurationPath, ConfigurationValue> FlattenJson(JToken token, string separator)
    {
        IDictionary<ConfigurationPath, ConfigurationValue> json = new Dictionary<ConfigurationPath, ConfigurationValue>();
        FlattenToken(token, json, ConfigurationPath.Empty, separator);
        return json;
    }

    public static void FlattenToken(JToken token, IDictionary<ConfigurationPath, ConfigurationValue> result, ConfigurationPath prefix, string separator)
    {
        ConfigurationPath GetPath<T>(T obj) => prefix.IsEmpty ? obj.ToString() : ConfigurationPath.Combine(separator, prefix, obj.ToString());

        switch(token.Type)
        {
            case JTokenType.Object:
            foreach(var property in token.Children<JProperty>()) FlattenToken(property.Value, result, GetPath(property.Name), separator);
            break;
            case JTokenType.Array:
            int index = 0;
            foreach(var value in token.Children())
            {
                FlattenToken(value, result, GetPath(index), separator);
                index++;
            }
            break;
            default:
            result.Add(prefix, token.ToString());
            break;
        }
    }
}
