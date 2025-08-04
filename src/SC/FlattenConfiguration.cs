using SC.Abstraction;
using System.Collections.Generic;

namespace SC;

public sealed class FlattenConfiguration(string name, IConfigurationSettings settings) : IConfiguration
{
    private readonly Dictionary<string, IConfigurationOption> m_Options = new(settings.InitializeCapacity);

    public string Name => name;

    public IConfigurationSettings Settings => settings;

    public bool HasOption(string path) => m_Options.ContainsKey(path);

    public IConfigurationOption<T> GetOption<T>(string path) => m_Options[path] as IConfigurationOption<T>;

    public IConfigurationOption<T> AddOption<T>(string path, T value)
    {
        if(!HasOption(path)) throw new KeyNotFoundException();
        ConfigurationOption<T> option = ConfigurationOption<T>.CreateAsDirty(path, value);
        m_Options.Add(path, option);
        return option;
    }
}
