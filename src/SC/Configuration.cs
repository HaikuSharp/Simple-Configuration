using SC.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SC;

public sealed class Configuration(string name, IRawProvider rawProvider, IConfigurationSettings settings) : IConfiguration
{
    private readonly Dictionary<string, IConfigurationOption> m_Options = new(settings.InitializeCapacity);

    public string Name => name;

    public IConfigurationSettings Settings => settings;

    public IEnumerable<IConfigurationOption> LoadedOptions => m_Options.Values;

    public bool HasOption(string path) => m_Options.ContainsKey(path) || rawProvider.HasRaw(path);

    public IConfigurationOption<T> GetOption<T>(string path) => m_Options.TryGetValue(path, out var loadedOption) ? loadedOption as IConfigurationOption<T> : InternalVerifyAndAddRawOption<T>(path);

    public IConfigurationOption<T> AddOption<T>(string path, T value) => InternalAddOption(path, value, !rawProvider.HasRaw(path));

    private ConfigurationOption<T> InternalAddOption<T>(string path, T value, bool isDirty)
    {
        ConfigurationOption<T> option = ConfigurationOption<T>.Create(path, value, isDirty);
        m_Options.Add(path, option);
        return option;
    }

    private ConfigurationOption<T> InternalVerifyAndAddRawOption<T>(string path) => !LoadedOptions.Any(o => path.StartsWith(o.Path)) ? InternalAddRawOption<T>(path) : throw new InvalidOperationException();

    private ConfigurationOption<T> InternalAddRawOption<T>(string path) => rawProvider.TryGetRaw<T>(path, out var rawValue) ? InternalAddOption(path, rawValue, false) : null;

    private class ConfigurationOption<T>(string path, T value) : IConfigurationOption<T>
    {
        public string Path => path;

        public int Version { get; private set; }

        public T Value
        {
            get => field;
            set
            {
                if(EqualityComparer<T>.Default.Equals(field, value)) return;
                field = value;
                Version++;
            }
        } = value;

        public static ConfigurationOption<T> Create(string path, T value, bool isDirty)
        {
            ConfigurationOption<T> option = new(path, value);
            if(isDirty) option.Version++;
            return option;
        }

        object IConfigurationOption.Value => Value;
    }
}
