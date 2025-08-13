using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SC.Abstraction;
using Sugar.Object.Extensions;
using System.IO;
using System.Threading.Tasks;

namespace SC.Newtonsoft.JSON;

public class JsonFileConfigurationSource(string filePath) : ConfigurationSourceBase(Path.GetFileNameWithoutExtension(filePath))
{
    protected override IConfigurationValueSource GetValueSource(IConfigurationSettings settings) => new JsonFileConfigurationValueSource(filePath, settings);

    private class JsonFileConfigurationValueSource(string filePath, IConfigurationSettings settings) : IConfigurationValueSource
    {
        private JToken m_Source;

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
            return token is not null ? token.ToObject<T>() : default;
        }

        public void SetRaw<T>(string path, T rawValue) => InternalGetOrCreateRawJsonValue(path).Replace(JToken.FromObject(rawValue));

        public void RemoveRaw(string path) => InternalGetRawJsonValue(path)?.Remove();

        private JToken InternalGetOrCreateRawJsonValue(string path)
        {
            var currentToken = m_Source;

            if(currentToken is null) return null;
            if(string.IsNullOrWhiteSpace(path)) return currentToken;
            if(path.IndexOf(settings.Separator) is -1) return currentToken.SelectToken(path);

            foreach(string pathPart in InternalGetPathEnumerator(path)) currentToken = currentToken.SelectToken(pathPart) ?? (currentToken[pathPart] = new JObject());

            return currentToken;
        }

        private JToken InternalGetRawJsonValue(string path)
        {
            var currentToken = m_Source;

            if(currentToken is null) return null;
            if(string.IsNullOrWhiteSpace(path)) return currentToken;
            if(path.IndexOf(settings.Separator) is -1) return currentToken.SelectToken(path);

            foreach(string pathPart in InternalGetPathEnumerator(path))
            {
                currentToken = currentToken.SelectToken(pathPart);
                if(currentToken is null) break;
            }

            return currentToken;
        }

        private ConfigurationPathEnumerator InternalGetPathEnumerator(string path) => new(path, settings.Separator);

        public void Load()
        {
            if(!File.Exists(filePath)) throw new FileNotFoundException(filePath);

            using StreamReader streamReader = new(filePath);
            using JsonTextReader jsonReader = new(streamReader);
            m_Source = JToken.Load(jsonReader);
        }

        public void Save()
        {
            if(m_Source == null) return;

            string directory = Path.GetDirectoryName(filePath);

            if(!string.IsNullOrEmpty(directory) && !Directory.Exists(directory)) Directory.CreateDirectory(directory).Forget();

            using StreamWriter streamWriter = new(filePath);
            using JsonTextWriter jsonWriter = new(streamWriter)
            {
                Formatting = Formatting.Indented
            };

            m_Source.WriteTo(jsonWriter);
        }

        public async Task LoadAsync()
        {
            if(!File.Exists(filePath)) throw new FileNotFoundException(filePath);

            using FileStream fileStream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);

            using StreamReader streamReader = new(fileStream);
            using JsonTextReader jsonReader = new(streamReader);
            m_Source = await JToken.LoadAsync(jsonReader).ConfigureAwait(false);
        }

        public async Task SaveAsync()
        {
            if(m_Source == null) return;

            string directory = Path.GetDirectoryName(filePath);

            if(!string.IsNullOrEmpty(directory) && !Directory.Exists(directory)) _ = Directory.CreateDirectory(directory);

            using FileStream fileStream = new(filePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);

            using StreamWriter streamWriter = new(fileStream);
            using JsonTextWriter jsonWriter = new(streamWriter)
            {
                Formatting = Formatting.Indented
            };

            await m_Source.WriteToAsync(jsonWriter).ConfigureAwait(false);
        }
    }
}
