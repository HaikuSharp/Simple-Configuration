using Newtonsoft.Json.Linq;
using SC.Abstraction;

namespace SC.Newtonsoft.JSON;

public class JsonConfigurationSource(string name, JToken token) : ConfigurationSourceBase(name)
{
    protected override IRawProvider GetRawProvider(IConfigurationSettings settings) => new JsonRawProvider(token, settings);

    private class JsonRawProvider(JToken token, IConfigurationSettings settings) : IRawProvider
    {
        public bool HasRaw(string path) => InternalGetRawJsonValue(path) is not null;

        public bool TryGetRaw<T>(string path, out T rawValue)
        {
            var token = InternalGetRawJsonValue(path);

            if(token is not null)
            {
                rawValue = token.ToObject<T>();
                return true;
            }

            rawValue = default;
            return false;
        }

        public T GetRaw<T>(string path)
        {
            var token = InternalGetRawJsonValue(path);
            return token is null ? default : token.ToObject<T>();
        }

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
