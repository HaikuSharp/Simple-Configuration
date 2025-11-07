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

    /// <summary>
    /// Gets the root values source token.
    /// </summary>
    protected JToken Source
    { 
        get => field ??= new JObject(); 
        set; 
    } = source;

    /// <inheritdoc/>
    public bool HasRaw(string path) => GetRawJsonValue(path) is not null;

    /// <inheritdoc/>
    public IEnumerable<string> GetRawsNames(string path) => GetRawJsonValue(path)?.Children<JProperty>().Select(p => p.Name) ?? [];

    /// <inheritdoc/>
    public bool TryGetRaw<T>(string path, out T rawValue)
    {
        var token = GetRawJsonValue(path);

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
        var token = GetRawJsonValue(path);
        return token is not null ? token.ToObject<T>() : default;
    }

    /// <inheritdoc/>
    public void SetRaw<T>(string path, T rawValue) => ReplaceToken(path, JToken.FromObject(rawValue));

    /// <inheritdoc/>
    public void RemoveRaw(string path)
    {
        m_TokensCache.Clear();

        if(string.IsNullOrEmpty(path)) Source = new JObject();
        else GetRawJsonValueWithoutCacheUpdate(path).Remove();
    }

    private void ReplaceToken(string path, JToken rtoken)
    {
        m_TokensCache.Clear();

        if(string.IsNullOrEmpty(path))
        {
            Source = rtoken;
            return;
        }

        var token = GetOrCreateRawJsonValue(path);

        token.Replace(rtoken);
        m_TokensCache[path] = token;
    }

    private JToken GetOrCreateRawJsonValue(string path)
    {
        var currentToken = Source;

        if(string.IsNullOrEmpty(path)) return currentToken;
        if(path.IndexOf(settings.Separator) is -1) return GetNotNullTokenFromToken(currentToken, path);

        foreach(string pathPart in InternalGetPathEnumerator(path)) currentToken = GetNotNullTokenFromToken(currentToken, pathPart);

        return currentToken;
    }

    private JToken GetRawJsonValue(string path) => TryGetCachedToken(path, out var token) ? token : (m_TokensCache[path] = FindRawJsonValue(path));

    private JToken GetRawJsonValueWithoutCacheUpdate(string path) => TryGetCachedToken(path, out var token) ? token : FindRawJsonValue(path);

    private bool TryGetCachedToken(string path, out JToken token)
    {
        if(string.IsNullOrEmpty(path))
        {
            token = Source;
            return true;
        }

        return m_TokensCache.TryGetValue(path, out token);
    }

    private JToken FindRawJsonValue(string path)
    {
        var currentToken = Source;

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
}
