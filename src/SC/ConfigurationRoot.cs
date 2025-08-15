using SC.Abstraction;
using Sugar.Object.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SC;

/// <inheritdoc cref="IConfigurationRoot"/>
public sealed class ConfigurationRoot(string name, IConfigurationSettings settings) : IConfigurationRoot
{
    private readonly Dictionary<string, IConfiguration> m_Configurations = [];

    /// <inheritdoc/>
    public string Name => name;

    /// <inheritdoc/>
    public IConfigurationSettings Settings => settings;

    /// <inheritdoc/>
    public IEnumerable<IConfigurationOption> LoadedOptions => m_Configurations.Values.SelectMany(c => c.LoadedOptions);

    /// <inheritdoc/>
    public IEnumerable<IConfiguration> LoadedConfigurations => m_Configurations.Values;

    /// <inheritdoc/>
    IEnumerable<IReadOnlyConfigurationOption> IReadOnlyConfiguration.LoadedOptions => LoadedOptions;

    /// <inheritdoc/>
    public bool HasOption(string path) => InternalGetConfiguration(path, out string optionPath)?.HasOption(optionPath) ?? false;

    /// <inheritdoc/>
    public IConfigurationOption<T> GetOption<T>(string path) => InternalGetConfiguration(path, out string optionPath)?.GetOption<T>(optionPath);

    /// <inheritdoc/>
    public IConfigurationOption<T> AddOption<T>(string path, T value) => InternalGetConfiguration(path, out string optionPath)?.AddOption(optionPath, value);

    /// <inheritdoc/>
    public void RemoveOption(string path) => InternalGetConfiguration(path, out string optionPath)?.RemoveOption(optionPath);

    /// <inheritdoc/>
    public bool HasConfiguration(string name) => m_Configurations.ContainsKey(name);

    /// <inheritdoc/>
    public IConfiguration GetConfiguration(string name) => m_Configurations.TryGetValue(name, out var configuration) ? configuration : null;

    /// <inheritdoc/>
    public void AddConfiguration(IConfiguration configuration) => m_Configurations.Add(configuration.Name, configuration);

    /// <inheritdoc/>
    public void RemoveConfiguration(string name) => m_Configurations.Remove(name).Forget();

    /// <inheritdoc/>
    public void Save(string path)
    {
        if(!string.IsNullOrEmpty(path))
        {
            InternalGetConfiguration(path, out string optionPath).Save(optionPath);
            return;
        }

        foreach(var configuration in m_Configurations.Values) configuration.Save(null);
    }

    /// <inheritdoc/>
    public void Load(string path)
    {
        if(!string.IsNullOrEmpty(path))
        {
            InternalGetConfiguration(path, out string optionPath).Load(optionPath);
            return;
        }

        foreach(var configuration in m_Configurations.Values) configuration.Load(null);
    }

    /// <inheritdoc/>
    public async Task SaveAsync(string path)
    {
        if(!string.IsNullOrEmpty(path))
        {
            await InternalGetConfiguration(path, out string optionPath).SaveAsync(optionPath);
            return;
        }

        foreach(var configuration in m_Configurations.Values) await configuration.SaveAsync(null);
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    IReadOnlyConfigurationOption<T> IReadOnlyConfiguration.GetOption<T>(string path) => GetOption<T>(path);
}