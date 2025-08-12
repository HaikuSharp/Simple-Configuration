using Newtonsoft.Json.Linq;
using SC.Abstraction;
using System;

namespace SC.Newtonsoft.JSON;

public class JsonConfigurationSource(string name, JToken token) : ConfigurationSourceBase(name)
{
    protected override IRawProvider GetRawProvider(IConfigurationSettings settings) => new JsonRawProvider(token, settings);

    private class JsonRawProvider(JToken token, IConfigurationSettings settings) : IRawProvider
    {
        public bool HasRaw(string path) => InternalGetRawJsonValue(path) is not null;

        public bool TryGetRaw(string path, Type type, out object rawValue)
        {
            rawValue = GetRaw(path, type);
            return rawValue is not null;
        }

        public object GetRaw(string path, Type type) => InternalGetRawJsonValue(path)?.ToObject(type);

        public void SetRaw(string path, object rawValue) => InternalGetRawJsonValue(path)?.Replace(JToken.FromObject(rawValue));

        private JToken InternalGetRawJsonValue(string path)
        {
            var currentToken = token;

            if(string.IsNullOrWhiteSpace(path)) return currentToken;
            if(path.IndexOf(settings.Separator) is -1) return currentToken.SelectToken(path);

            foreach(string pathPart in InternalGetPathEnumerator(path))
            {
                currentToken = currentToken.SelectToken(path);
                if(currentToken is null) break;
            }

            return currentToken;
        }

        private ConfigurationPathEnumerator InternalGetPathEnumerator(string path) => new(path, settings.Separator);
    }
}
