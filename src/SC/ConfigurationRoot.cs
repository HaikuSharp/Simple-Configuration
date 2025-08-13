using SC.Abstraction;
using Sugar.Object.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SC;

public class ConfigurationRoot(string name, IConfigurationSettings settings) : IConfigurationRoot
{
    private readonly Dictionary<string, IConfiguration> m_Configurations = [];

    public string Name => name;

    public IConfigurationSettings Settings => settings;

    public IEnumerable<IConfigurationOption> LoadedOptions => m_Configurations.Values.SelectMany(c => c.LoadedOptions);

    public bool HasOption(string path) => InternalGetConfiguration(path, out string optionPath)?.HasOption(optionPath) ?? false;

    public IConfigurationOption<T> GetOption<T>(string path) => InternalGetConfiguration(path, out string optionPath)?.GetOption<T>(optionPath);

    public IConfigurationOption<T> AddOption<T>(string path, T value) => InternalGetConfiguration(path, out string optionPath)?.AddOption(optionPath, value);

    public void RemoveOption(string path) => InternalGetConfiguration(path, out string optionPath)?.RemoveOption(optionPath);

    public bool HasConfiguration(string name) => m_Configurations.ContainsKey(name);

    public IConfiguration GetConfiguration(string name) => m_Configurations.TryGetValue(name, out var configuration) ? configuration : null;

    public void AddConfiguration(IConfiguration configuration) => m_Configurations.Add(configuration.Name, configuration);

    public void RemoveConfiguration(string name) => m_Configurations.Remove(name).Forget();

    public void Save(string path)
    {
        if(!string.IsNullOrEmpty(path))
        {
            InternalGetConfiguration(path, out string optionPath).Save(optionPath);
            return;
        }

        foreach(var configuration in m_Configurations.Values) configuration.Save(null);
    }

    public void Load(string path)
    {
        if(!string.IsNullOrEmpty(path))
        {
            InternalGetConfiguration(path, out string optionPath).Load(optionPath);
            return;
        }

        foreach(var configuration in m_Configurations.Values) configuration.Load(null);
    }

    public async Task SaveAsync(string path)
    {
        if(!string.IsNullOrEmpty(path))
        {
            await InternalGetConfiguration(path, out string optionPath).SaveAsync(optionPath);
            return;
        }

        foreach(var configuration in m_Configurations.Values) await configuration.SaveAsync(null);
    }

    public async Task LoadAsync(string path)
    {
        if(!string.IsNullOrEmpty(path))
        {
            await InternalGetConfiguration(path, out string optionPath).LoadAsync(optionPath);
            return;
        }

        foreach(var configuration in m_Configurations.Values) await configuration.LoadAsync(null);
    }

#pragma warning disable IDE0079
#pragma warning disable IDE0057

    private IConfiguration InternalGetConfiguration(string path, out string optionPath)
    {
        int firstSeparatorIndex = path.IndexOf(Settings.Separator);

        if(firstSeparatorIndex is -1)
        {
            optionPath = string.Empty;
            return m_Configurations[path];
        }

        optionPath = path.Substring(firstSeparatorIndex + 1, path.Length - (firstSeparatorIndex - 1));
        return m_Configurations[path.Substring(0, firstSeparatorIndex)];
    }

#pragma warning restore IDE0057
#pragma warning restore IDE0079
}