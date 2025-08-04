using Newtonsoft.Json.Linq;
using SC.Abstraction;
using System.Collections.Generic;

namespace SC.Newtonsoft.JSON;

public class JsonConfiguration(string name, JToken jToken, IConfigurationSettings settings) : IConfiguration
{
    private readonly JToken m_JsonRootToken = jToken;
    private readonly Dictionary<string, IConfigurationOption> m_LoadedOptions = [];

    public string Name => name;

    public IConfigurationSettings Settings => settings;

    public bool HasOption(string path) => m_LoadedOptions.ContainsKey(path) || InternalHasOptionInRawJson(path);

    public IConfigurationOption<T> GetOption<T>(string path) => m_LoadedOptions.TryGetValue(path, out var option) ? option as IConfigurationOption<T> : InternalAddOption(path, InternalGetRawJsonValue(path).ToObject<T>(), false);

    public IConfigurationOption<T> AddOption<T>(string path, T value) => InternalAddOption(path, value, true);

    private ConfigurationOption<T> InternalAddOption<T>(string path, T value, bool isDirty)
    {
        var option = isDirty ? ConfigurationOption<T>.CreateAsDirty(path, value) : new ConfigurationOption<T>(path, value);
        m_LoadedOptions.Add(path, option);
        return option;
    }

    private bool InternalHasOptionInRawJson(string path) => InternalGetRawJsonValue(path) is not null;

    private JToken InternalGetRawJsonValue(string path)
    {
        JToken currentToken = m_JsonRootToken;

        if(string.IsNullOrWhiteSpace(path)) return currentToken;
        if(path.IndexOf(Settings.Separator) is -1) return currentToken.SelectToken(path);

        foreach(var pathPart in InternalGetPathEnumerator(path))
        {
            currentToken = currentToken.SelectToken(path);
            if(currentToken is null) break;
        }

        return currentToken;
    }

    private ConfigurationPathEnumerator InternalGetPathEnumerator(string path) => new(path, Settings.Separator);
}
