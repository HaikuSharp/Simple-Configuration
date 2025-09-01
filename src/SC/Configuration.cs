using SC.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SC;

/// <inheritdoc cref="IConfiguration"/>
public sealed class Configuration(string name, IConfigurationSettings settings) : IConfiguration
{
    private readonly Dictionary<string, IConfigurationOption> m_Options = new(settings.InitializeCapacity);

    /// <inheritdoc/>
    public string Name => name;

    /// <inheritdoc/>
    public IConfigurationSettings Settings => settings;

    /// <inheritdoc/>
    public IEnumerable<IConfigurationOption> Options => m_Options.Values;

    /// <inheritdoc/>
    IEnumerable<IReadOnlyConfigurationOption> IReadOnlyConfiguration.Options => Options;

    /// <inheritdoc/>
    public bool HasOption(string path) => m_Options.ContainsKey(path);

    /// <inheritdoc/>
    public IEnumerable<string> GetOptionsNames(string path) => InternalGetOptionsNames(path);

    /// <inheritdoc/>
    public IConfigurationOption<T> GetOption<T>(string path) => m_Options.TryGetValue(path, out var loadedOption) ? loadedOption as IConfigurationOption<T> : null;

    /// <inheritdoc/>
    public IConfigurationOption<T> AddOption<T>(string path, T value) => InternalAddOption(path, value);

    /// <inheritdoc/>
    public void RemoveOption(string path) => _ = m_Options.Remove(path);

    /// <inheritdoc/>
    public void Save(string path, IConfigurationValueSource source) => InternalSaveOptions(path, source);

    /// <inheritdoc/>
    public void Load(string path, IConfigurationValueSource source) => InternalLoadOptions(path, source);

    private void InternalSaveOptions(string path, IConfigurationValueSource source)
    {
        if(string.IsNullOrEmpty(path))
        {
            foreach(var option in Options) option.Save(source);
            return;
        }

        foreach(var option in Options)
        {
            if(!option.Path.StartsWith(path)) continue;
            option.Save(source);
        }
    }

    private void InternalLoadOptions(string path, IConfigurationValueSource source)
    {
        if(string.IsNullOrEmpty(path))
        {
            foreach(var option in Options) option.Load(source);
            return;
        }

        foreach(var option in Options)
        {
            if(!option.Path.StartsWith(path)) continue;
            option.Load(source);
        }
    }

    private ConfigurationOption<T> InternalAddOption<T>(string path, T value)
    {
        ConfigurationOption<T> option = new(path, value);
        m_Options.Add(path, option);
        return option;
    }

    private IEnumerable<string> InternalGetOptionsNames(string path) => string.IsNullOrEmpty(path) ? m_Options.Keys : m_Options.Keys.Where(k => k.StartsWith(path));

    /// <inheritdoc/>
    IReadOnlyConfigurationOption<T> IReadOnlyConfiguration.GetOption<T>(string path) => GetOption<T>(path);

    private sealed class ConfigurationOption<T>(string path, T optionValue) : IConfigurationOption<T>
    {
        public string Path => path;

        public Type ValueType => typeof(T);

        public T Value { get; set; } = optionValue;

        object IReadOnlyConfigurationOption.Value => Value;

        object IConfigurationOption.Value
        {
            get => Value;
            set => Value = (T)value;
        }

        public void Save(IConfigurationValueSource valueSource) => valueSource.SetRaw(Path, Value);

        public void Load(IConfigurationValueSource valueSource)
        {
            if(!valueSource.TryGetRaw<T>(Path, out var value)) return;
            Value = value;
        }

        public override string ToString() => $"({Path}, {Value})";
    }
}
