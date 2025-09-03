using SC.Abstraction;
using SC.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SC;

/// <inheritdoc cref="IConfiguration"/>
public sealed class Configuration(string name, IConfigurationSettings settings) : IConfiguration
{
    private IConfigurationValueSource m_LoadedSource;
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
    public bool HasLoadedSource => m_LoadedSource is not null;

    /// <inheritdoc/>
    public bool HasOption(string path) => m_Options.ContainsKey(path) || (TryGetLoadedSource(out var source) && source.HasRaw(path));

    /// <inheritdoc/>
    public IEnumerable<string> GetOptionsNames(string path)
    {
        var optionsNames = InternalGetOptionsNames(path);
        return TryGetLoadedSource(out var source) ? optionsNames.Concat(source.GetRawsNames(path)) : optionsNames;
    }

    /// <inheritdoc/>
    public IConfigurationOption<T> GetOption<T>(string path) => m_Options.TryGetValue(path, out var loadedOption) ? loadedOption as IConfigurationOption<T> : InternalVerifyAndAddRawOption<T>(path);

    /// <inheritdoc/>
    public IConfigurationOption<T> AddOption<T>(string path, T value) => InternalAddOption(path, value);

    /// <inheritdoc/>
    public void RemoveOption(string path)
    {
        _ = m_Options.Remove(path);

        if(!TryGetLoadedSource(out var source)) return;

        source.RemoveRaw(path);
    }

    /// <inheritdoc/>
    public void Save(string path, IConfigurationValueSource source) => InternalSaveOptions(path, source);

    /// <inheritdoc/>
    public void Load(string path, IConfigurationValueSource source) => InternalLoadOptions(path, source);

    private void InternalSaveOptions(string path, IConfigurationValueSource source)
    {
        source = GetSource(source);

        if(string.IsNullOrEmpty(path)) InternalSaveAllOptions(source);
        else InternalSaveOptionsWithPath(path, source);
    }

    private void InternalSaveOptionsWithPath(string path, IConfigurationValueSource source)
    {
        foreach(var option in Options)
        {
            if(!option.Path.StartsWith(path)) continue;
            option.Save(source);
        }
    }

    private void InternalSaveAllOptions(IConfigurationValueSource source)
    {
        foreach(var option in Options) option.Save(source);
    }

    private void InternalLoadOptions(string path, IConfigurationValueSource source)
    {
        source = GetSource(source);

        if(string.IsNullOrEmpty(path)) InternalLoadAllOptions(source);
        else InternalLoadOptionsWithPath(path, source);

        m_LoadedSource = source;
    }

    private void InternalLoadOptionsWithPath(string path, IConfigurationValueSource source)
    {
        foreach(var option in Options)
        {
            if(!option.Path.StartsWith(path)) continue;
            option.Load(source);
        }
    }

    private void InternalLoadAllOptions(IConfigurationValueSource source)
    {
        foreach(var option in Options) option.Load(source);
    }

    private ConfigurationOption<T> InternalVerifyAndAddRawOption<T>(string path) => !Options.Any(o => path.StartsWith(o.Path)) ? InternalAddRawOption<T>(path) : throw new InvalidOperationException();

    private ConfigurationOption<T> InternalAddRawOption<T>(string path) => TryGetLoadedSource(out var source) && source.TryGetRaw(path, out T raw) ? InternalAddOption(path, raw) : null;

    private ConfigurationOption<T> InternalAddOption<T>(string path, T value)
    {
        ConfigurationOption<T> option = new(path, GetOptionName(path), value);
        m_Options.Add(path, option);
        return option;
    }

    private string GetOptionName(string path)
    {
        int separatorLastIndex = path.LastIndexOf(Settings.Separator);

#pragma warning disable IDE0079
#pragma warning disable IDE0057

        return separatorLastIndex is -1 ? path : path.Substring(0, separatorLastIndex);

#pragma warning restore IDE0057
#pragma warning restore IDE0079
    }

    private bool TryGetLoadedSource(out IConfigurationValueSource source) => (source = m_LoadedSource) is not null;

#pragma warning disable IDE0079
#pragma warning disable IDE0057

    private IEnumerable<string> InternalGetOptionsNames(string path) => string.IsNullOrEmpty(path) ? m_Options.Keys : m_Options.Keys.Where(k => k.StartsWith(path)).Select(k => k.GetSectionName(path, Settings.Separator));

#pragma warning restore IDE0057
#pragma warning restore IDE0079

    private IConfigurationValueSource GetSource(IConfigurationValueSource source) => (source ??= m_LoadedSource) is not null ? source : throw new NullReferenceException("The value source cannot be null or not loaded previously.");

    /// <inheritdoc/>
    IReadOnlyConfigurationOption<T> IReadOnlyConfiguration.GetOption<T>(string path) => GetOption<T>(path);

    private sealed class ConfigurationOption<T>(string path, string name, T optionValue) : IConfigurationOption<T>
    {
        public string Name => name;

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
