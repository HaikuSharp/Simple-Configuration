using Newtonsoft.Json.Linq;
using SC.Abstraction;
using SC.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SC.Newtonsoft.JSON;

public class JsonConfigurationValueSource(JToken source, IConfigurationSettings settings) : IConfigurationValueSource
{
    private readonly Dictionary<string, JToken> m_TokensCache = [];

    internal JToken NotNullSource => source ??= new JObject();

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
    }

    /// <inheritdoc/>
    public void RemoveRaw(string path)
    {
        InternalGetRawJsonValueWithoutCacheUpdate(path)?.Remove();
        _ = m_TokensCache.Remove(path);
    }

    /// <inheritdoc/>
    public void Clear()
    {
        source = null;
        m_TokensCache.Clear();
    }

    private JToken InternalGetOrCreateRawJsonValue(string path)
    {
        var currentToken = NotNullSource;

        if(string.IsNullOrWhiteSpace(path)) return currentToken;
        if(path.IndexOf(settings.Separator) is -1) return currentToken[path] ??= new JObject();

        foreach(string pathPart in InternalGetPathEnumerator(path)) currentToken = currentToken[pathPart] ??= new JObject();

        return currentToken;
    }

    private JToken InternalGetRawJsonValue(string path) => m_TokensCache.TryGetValue(path, out var token) ? token : (m_TokensCache[path] = InternalFindRawJsonValue(path));

    private JToken InternalGetRawJsonValueWithoutCacheUpdate(string path) => m_TokensCache.TryGetValue(path, out var token) ? token : InternalFindRawJsonValue(path);

    private JToken InternalFindRawJsonValue(string path)
    {
        var currentToken = NotNullSource;

        if(string.IsNullOrWhiteSpace(path)) return currentToken;
        if(path.IndexOf(settings.Separator) is -1) return currentToken[path];

        foreach(string pathPart in InternalGetPathEnumerator(path))
        {
            currentToken = currentToken[pathPart];
            if(currentToken is null) break;
        }

        return currentToken;
    }

    private ConfigurationPathEnumerator InternalGetPathEnumerator(string path) => path.AsPathEnumerator(settings.Separator);

    /// <inheritdoc/>
    public void Load() { }

    /// <inheritdoc/>
    public void Save() { }

    /// <inheritdoc/>
    public Task LoadAsync() => Task.CompletedTask;

    /// <inheritdoc/>
    public Task SaveAsync() => Task.CompletedTask;

    /// <inheritdoc/>
    public void RemoveExcept(params IEnumerable<string> paths)
    {
        if(!paths.Any())
        {
            Clear();
            return;
        }

        RemoveExcept(NotNullSource, paths);
    }

    private void RemoveExcept(JToken token, IEnumerable<string> paths) => RemoveExcept(token, paths, string.Empty);

    private void RemoveExcept(JToken token, IEnumerable<string> paths, string currentPath)
    {
        switch(token.Type)
        {
            case JTokenType.Object:
            var obj = token as JObject;
            var properties = obj.Properties();

            foreach(var property in properties)
            {
                if(paths.Any(path => IsInPath(path, settings.CombinePaths(currentPath, property.Name)))) RemoveExcept(property.Value, paths, settings.CombinePaths(currentPath, property.Name));
                else obj.Remove(property.Name);
            }

            break;

            case JTokenType.Array:
            var array = token as JArray;

            for(int i = 0; i < array.Count; i++) RemoveExcept(array[i], paths, settings.CombinePaths(currentPath, i.ToString()));

            break;
        }
    }

    private bool IsInPath(string path, string targetPath) => path == targetPath || targetPath.StartsWith(settings.Separator);
}
