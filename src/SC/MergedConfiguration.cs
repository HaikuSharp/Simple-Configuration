using SC.Abstraction;
using System.Collections.Generic;

namespace SC;

public class MergedConfiguration(string name, IConfigurationSettings settings) : IConfiguration
{
    private readonly Dictionary<string, IConfiguration> m_Configurations = [];

    public string Name => name;

    public IConfigurationSettings Settings => settings;

    public bool HasOption(string path) => InternalGetConfiguration(path, out var optionPath)?.HasOption(optionPath) ?? false;

    public IConfigurationOption<T> GetOption<T>(string path) => InternalGetConfiguration(path, out var optionPath)?.GetOption<T>(optionPath);

    public IConfigurationOption<T> AddOption<T>(string path, T value) => InternalGetConfiguration(path, out var optionPath)?.AddOption(optionPath, value);

    public void AddConfiguration(IConfiguration configuration) => m_Configurations.Add(configuration.Name, configuration);

#pragma warning disable IDE0079
#pragma warning disable IDE0057

    private IConfiguration InternalGetConfiguration(string path, out string optionPath)
    {
        var firstSeparatorIndex = path.IndexOf(Settings.Separator);

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