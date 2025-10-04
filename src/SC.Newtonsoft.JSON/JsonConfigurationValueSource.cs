using Newtonsoft.Json.Linq;
using SC.Abstraction;
using SC.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SC.Newtonsoft.JSON;

/// <summary>
/// Represents a configuration values source that loads from and saves to Json token.
/// </summary>
public class JsonConfigurationValueSource(JToken source, IConfigurationSettings settings) : IConfigurationValueSource
{
    private readonly Dictionary<string, JToken> m_TokensCache = [];
    private JToken m_Source = source;

    internal JToken NotNullSource => m_Source ??= new JObject();

    /// <inheritdoc/>
    public bool HasRaw(string path) => InternalGetRawJsonValue(path) is not null;

    /// <inheritdoc/>
    public IEnumerable<string> GetRawsNames(string path) => InternalGetRawJsonValue(path)?.Children<JProperty>().Select(p => p.Name) ?? [];

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
        m_TokensCache.Clear();
    }

    /// <inheritdoc/>
    public void RemoveRaw(string path)
    {
        if(string.IsNullOrEmpty(path)) m_Source?.Replace(new JObject());
        else InternalGetRawJsonValueWithoutCacheUpdate(path)?.Remove();
        m_TokensCache.Clear();
    }

    private JToken InternalGetOrCreateRawJsonValue(string path)
    {
        var currentToken = NotNullSource;

        if(string.IsNullOrEmpty(path)) return currentToken;
        if(path.IndexOf(settings.Separator) is -1) return GetNotNullTokenFromToken(currentToken, path);

        foreach(string pathPart in InternalGetPathEnumerator(path)) currentToken = GetNotNullTokenFromToken(currentToken, pathPart);

        return currentToken;
    }

    private JToken InternalGetRawJsonValue(string path) => TryGetCachedToken(path, out var token) ? token : (m_TokensCache[path] = InternalFindRawJsonValue(path));

    private JToken InternalGetRawJsonValueWithoutCacheUpdate(string path) => TryGetCachedToken(path, out var token) ? token : InternalFindRawJsonValue(path);

    private bool TryGetCachedToken(string path, out JToken token)
    {
        if(string.IsNullOrEmpty(path))
        {
            token = NotNullSource;
            return true;
        }

        return m_TokensCache.TryGetValue(path, out token);
    }

    private JToken InternalFindRawJsonValue(string path)
    {
        var currentToken = NotNullSource;

        if(string.IsNullOrEmpty(path)) return currentToken;
        if(path.IndexOf(settings.Separator) is -1) return currentToken[path];

        foreach(string pathPart in InternalGetPathEnumerator(path))
        {
            currentToken = currentToken[pathPart];
            if(currentToken is null) break;
        }

        return currentToken;
    }

    private ConfigurationPathEnumerator InternalGetPathEnumerator(string path) => path.AsPathEnumerator(settings.Separator);

    private static JToken GetNotNullTokenFromToken(JToken source, string path) => source[path] ??= new JObject();

    /// <inheritdoc/>
    public void Load() { }

    /// <inheritdoc/>
    public void Save() { }

    /// <inheritdoc/>
    public Task LoadAsync() => Task.CompletedTask;

    /// <inheritdoc/>
    public Task SaveAsync() => Task.CompletedTask;
}
