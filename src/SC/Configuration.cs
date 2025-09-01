using SC.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SC;

/// <inheritdoc cref="IConfiguration"/>
public sealed class Configuration(string name, IConfigurationValueSource valueSource, IConfigurationSettings settings) : IConfigurationRoot
{
    private readonly Dictionary<string, IConfigurationOption> m_Options = new(settings.InitializeCapacity);

    /// <inheritdoc/>
    public string Name => name;

    /// <inheritdoc/>
    public IConfigurationSettings Settings => settings;

    /// <inheritdoc/>
    public IEnumerable<IConfigurationOption> LoadedOptions => m_Options.Values;

    /// <inheritdoc/>
    IEnumerable<IReadOnlyConfigurationOption> IReadOnlyConfiguration.LoadedOptions => LoadedOptions;

    /// <inheritdoc/>
    public IConfigurationValueSource Source 
    { 
        get => valueSource; 
        set => valueSource = value; 
    }

    /// <inheritdoc/>
    public bool HasOption(string path) => m_Options.ContainsKey(path) || valueSource.HasRaw(path);

    /// <inheritdoc/>
    public IEnumerable<string> GetOptionsNames(string path) => InternalGetLoadedOptionsNames(path).Concat(valueSource.GetRawsNames(path)).Distinct();

    /// <inheritdoc/>
    public IConfigurationOption<T> GetOption<T>(string path) => m_Options.TryGetValue(path, out var loadedOption) ? loadedOption as IConfigurationOption<T> : InternalVerifyAndAddRawOption<T>(path);

    /// <inheritdoc/>
    public IConfigurationOption<T> AddOption<T>(string path, T value) => InternalAddOption(path, value);

    /// <inheritdoc/>
    public void RemoveOption(string path)
    {
        _ = m_Options.Remove(path);
        valueSource.RemoveRaw(path);
    }

    /// <inheritdoc/>
    public void Save(string path)
    {
        InternalSaveOptions(path);
        valueSource.Save();
    }

    /// <inheritdoc/>
    public void Load(string path)
    {
        valueSource.Load();
        InternalLoadOptions(path);
    }

    /// <inheritdoc/>
    public async Task SaveAsync(string path)
    {
        InternalSaveOptions(path);
        await valueSource.SaveAsync();
    }

    /// <inheritdoc/>
    public async Task LoadAsync(string path)
    {
        await valueSource.LoadAsync();
        InternalLoadOptions(path);
    }

    private void InternalSaveOptions(string path)
    {
        if(string.IsNullOrEmpty(path))
        {
            foreach(var option in LoadedOptions) option.Save(valueSource);
            return;
        }

        foreach(var option in LoadedOptions)
        {
            if(!option.Path.StartsWith(path)) continue;
            option.Save(valueSource);
        }
    }

    private void InternalLoadOptions(string path)
    {
        if(string.IsNullOrEmpty(path))
        {
            foreach(var option in LoadedOptions) option.Load(valueSource);
            return;
        }

        foreach(var option in LoadedOptions)
        {
            if(!option.Path.StartsWith(path)) continue;
            option.Load(valueSource);
        }
    }

    private ConfigurationOption<T> InternalAddOption<T>(string path, T value)
    {
        ConfigurationOption<T> option = new(path, value);
        m_Options.Add(path, option);
        return option;
    }

    private ConfigurationOption<T> InternalVerifyAndAddRawOption<T>(string path) => !LoadedOptions.Any(o => path.StartsWith(o.Path)) ? InternalAddRawOption<T>(path) : throw new InvalidOperationException();

    private ConfigurationOption<T> InternalAddRawOption<T>(string path) => valueSource.TryGetRaw(path, out T raw) ? InternalAddOption(path, raw) : null;

    private IEnumerable<string> InternalGetLoadedOptionsNames(string path) => string.IsNullOrEmpty(path) ? m_Options.Keys : m_Options.Keys.Where(k => k.StartsWith(path));

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
