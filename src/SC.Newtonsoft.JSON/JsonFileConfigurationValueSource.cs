using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SC.Abstraction;
using SC.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SC.Newtonsoft.JSON;

/// <summary>
/// Represents a configuration values source that loads from and saves to Json file.
/// </summary>
public class JsonFileConfigurationValueSource(string filePath, IConfigurationSettings settings) : IFileConfigurationValueSource
{
    private readonly Dictionary<string, JToken> m_TokensCache = [];
    private JToken m_Source;

    /// <inheritdoc/>
    public string FilePath => filePath;

    private JToken NotNullSource => m_Source ??= new JObject();

    /// <inheritdoc/>
    public bool HasRaw(string path) => InternalGetRawJsonValue(path) is not null;

    /// <inheritdoc/>
    public IEnumerable<string> GetRawsNames(string path) => InternalGetRawJsonValue(path)?.Children().OfType<JProperty>().Select(p => p.Name) ?? [];

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public T GetRaw<T>(string path)
    {
        var token = InternalGetRawJsonValue(path);
        return token is not null ? token.ToObject<T>() : default;
    }

    /// <inheritdoc/>
    public void SetRaw<T>(string path, T rawValue)
    {
        var token = InternalGetOrCreateRawJsonValue(path);
        token.Replace(JToken.FromObject(rawValue));
        m_TokensCache[path] = token;
    }

    /// <inheritdoc/>
    public void RemoveRaw(string path) => InternalGetRawJsonValue(path)?.Remove();

    private JToken InternalGetOrCreateRawJsonValue(string path)
    {
        var currentToken = NotNullSource;

        if(currentToken is null) return null;
        if(string.IsNullOrWhiteSpace(path)) return currentToken;
        if(path.IndexOf(settings.Separator) is -1) return currentToken.SelectToken(path);

        foreach(string pathPart in InternalGetPathEnumerator(path)) currentToken = currentToken.SelectToken(pathPart) ?? (currentToken[pathPart] = new JObject());

        return currentToken;
    }

    private JToken InternalGetRawJsonValue(string path) => m_TokensCache.TryGetValue(path, out var token) ? token : (m_TokensCache[path] = FindRawJsonValue(path));

    private JToken FindRawJsonValue(string path)
    {
        var currentToken = NotNullSource;

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

    private ConfigurationPathEnumerator InternalGetPathEnumerator(string path) => path.AsPathEnumerator(settings.Separator);

    /// <inheritdoc/>
    public void Load()
    {
        if(!File.Exists(filePath)) throw new FileNotFoundException(filePath);

        using StreamReader streamReader = new(filePath);
        using JsonTextReader jsonReader = new(streamReader);
        m_Source = JToken.Load(jsonReader);
        m_TokensCache.Clear();
    }

    /// <inheritdoc/>
    public void Save()
    {
        var source = NotNullSource;

        string directory = Path.GetDirectoryName(filePath);

        if(!string.IsNullOrEmpty(directory) && !Directory.Exists(directory)) _ = Directory.CreateDirectory(directory);

        using StreamWriter streamWriter = new(filePath);
        using JsonTextWriter jsonWriter = new(streamWriter)
        {
            Formatting = Formatting.Indented
        };

        source.WriteTo(jsonWriter);
    }

    /// <inheritdoc/>
    public async Task LoadAsync()
    {
        if(!File.Exists(filePath)) throw new FileNotFoundException(filePath);

        using FileStream fileStream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);

        using StreamReader streamReader = new(fileStream);
        using JsonTextReader jsonReader = new(streamReader);
        m_Source = await JToken.LoadAsync(jsonReader).ConfigureAwait(false);
        m_TokensCache.Clear();
    }

    /// <inheritdoc/>
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