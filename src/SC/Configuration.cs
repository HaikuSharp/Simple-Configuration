using SC.Abstraction;
using SC.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SC;

public sealed class Configuration(string name, IRawProvider rawProvider, IConfigurationSettings settings) : IConfiguration
{
    private readonly Dictionary<string, ConfigurationOptionBase> m_Options = new(settings.InitializeCapacity);

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

    private ConfigurationOption<T> InternalAddRawOption<T>(string path) => rawProvider.TryGetRaw(path, typeof(T), out object rawValue) ? InternalAddOption(path, (T)rawValue, false) : null;

    public void Save(string path)
    {
        foreach(var option in GetDirtyOptions(path))
        {
            option.Reset();
            rawProvider.SetRaw(option.Path, option.GetObject());
        }
    }

    public void Load(string path)
    {
        foreach(var option in GetOptions(path))
        {
            option.Reset();
            option.SetObject(rawProvider.GetRaw(option.Path, option.ValueType));
        }
    }

    private IEnumerable<ConfigurationOptionBase> GetDirtyOptions(string path) => GetOptions(path).Where(o => o.IsDirty());

    private IEnumerable<ConfigurationOptionBase> GetOptions(string path) => string.IsNullOrEmpty(path) ? m_Options.Values : m_Options.Values.Where(o => o.Path.StartsWith(path));

    private class ConfigurationOption<T>(string path, T value) : ConfigurationOptionBase(path), IConfigurationOption<T>
    {
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

        internal override Type ValueType => typeof(T);

        public static ConfigurationOption<T> Create(string path, T value, bool isDirty)
        {
            ConfigurationOption<T> option = new(path, value);
            if(isDirty) option.Version++;
            return option;
        }

        internal override object GetObject() => Value;

        internal override void SetObject(object obj) => Value = obj is T nvalue ? nvalue : throw new InvalidCastException();
    }

    private abstract class ConfigurationOptionBase(string path) : IConfigurationOption
    {
        public string Path => path;

        public int Version { get; protected set; }

        internal abstract Type ValueType { get; }

        object IConfigurationOption.Value => GetObject();

        internal abstract object GetObject();

        internal abstract void SetObject(object obj);

        internal void Reset() => Version = 0;
    }
}
