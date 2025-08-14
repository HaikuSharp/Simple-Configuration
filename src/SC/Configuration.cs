using SC.Abstraction;
using Sugar.Object.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SC;

public sealed class Configuration(string name, IConfigurationValueSource valueSource, IConfigurationSettings settings) : IConfiguration
{
    private readonly Dictionary<string, IConfigurationOption> m_Options = new(settings.InitializeCapacity);

    public string Name => name;

    public IConfigurationSettings Settings => settings;

    public IEnumerable<IConfigurationOption> LoadedOptions => m_Options.Values;

    IEnumerable<IReadOnlyConfigurationOption> IReadOnlyConfiguration.LoadedOptions => LoadedOptions;

    public bool HasOption(string path) => m_Options.ContainsKey(path) || valueSource.HasRaw(path);

    public IConfigurationOption<T> GetOption<T>(string path) => m_Options.TryGetValue(path, out var loadedOption) ? loadedOption as IConfigurationOption<T> : InternalVerifyAndAddRawOption<T>(path);

    public IConfigurationOption<T> AddOption<T>(string path, T value) => InternalAddOption(path, value);

    public void RemoveOption(string path)
    {
        m_Options.Remove(path).Forget();
        valueSource.RemoveRaw(path);
    }

    public void Save(string path)
    {
        InternalSaveOptions(path);
        valueSource.Save();
    }

    public void Load(string path)
    {
        valueSource.Load();
        InternalLoadOptions(path);
    }

    public async Task SaveAsync(string path)
    {
        InternalSaveOptions(path);
        await valueSource.SaveAsync();
    }

    public async Task LoadAsync(string path)
    {
        await valueSource.LoadAsync();
        InternalLoadOptions(path);
    }

    private void InternalSaveOptions(string path)
    {
        foreach(var option in LoadedOptions)
        {
            if(!option.Path.StartsWith(path)) return;
            option.Save(valueSource);
        }
    }

    private void InternalLoadOptions(string path)
    {
        foreach(var option in LoadedOptions)
        {
            if(!option.Path.StartsWith(path)) return;
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
