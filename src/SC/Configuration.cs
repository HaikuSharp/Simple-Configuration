using SC.Abstraction;
using SC.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SC;

/// <inheritdoc cref="IConfiguration"/>
public sealed class Configuration(IConfigurationSettings settings) : IConfiguration
{
    private readonly PathValidator m_Validator = new(settings);
    private readonly Dictionary<string, ConfigurationOptionEntry> m_Options = new(settings.InitializeCapacity);

    private IConfigurationValueSource m_LoadedSource;

    /// <inheritdoc/>
    public IConfigurationSettings Settings => settings;

    /// <inheritdoc/>
    public bool HasLoadedSource => m_LoadedSource is not null;

    /// <inheritdoc/>
    private IEnumerable<ConfigurationOptionEntry> Entries => m_Options.Values;

    /// <inheritdoc/>
    public bool HasOption(string path) => m_Options.ContainsKey(path) || (TryGetLoadedSource(out var source) && source.HasRaw(path));

    /// <inheritdoc/>
    public IEnumerable<string> GetOptionsNames(string path)
    {
        var names = Entries.Where(e => e.Path.StartsWith(path)).Select(e => e.Name);
        if(TryGetLoadedSource(out var source)) names = names.Concat(source.GetRawsNames(path)).Distinct();
        return names;
    }

    /// <inheritdoc/>
    public TOption GetOption<TOption>(string path) where TOption : class, IConfigurationOption, new()
    {
        if(m_Options.TryGetValue(path, out var entry)) return entry.Option as TOption;

        if(TryGetLoadedSource(out var source) && source.HasRaw(path))
        {
            var option = InternalAddOption<TOption>(path);
            option.Load(path, source);
            return option;
        }

        return null;
    }

    /// <inheritdoc/>
    public TOption AddOption<TOption>(string path) where TOption : class, IConfigurationOption, new() => HasOption(path) ? throw new InvalidOperationException() : InternalAddOption<TOption>(path);

    /// <inheritdoc/>
    public void RemoveOption(string path)
    {
        if(m_Options.Remove(path)) m_Validator.Remove(path);
        if(TryGetLoadedSource(out var source)) source.RemoveRaw(path);
    }

    /// <inheritdoc/>
    public void Save(string path, IConfigurationValueSource source)
    {
        if(!TryGetNotNullSource(source, out source)) return;

        foreach(var entry in GetEntriesByPath(path)) entry.Save(source);
    }

    /// <inheritdoc/>
    public void Load(string path, IConfigurationValueSource source)
    {
        if(!TryGetNotNullSource(source, out source)) return;

        foreach(var entry in GetEntriesByPath(path)) entry.Load(source);
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

    private TOption InternalAddOption<TOption>(string path) where TOption : class, IConfigurationOption, new()
    {
        ValidatePath(path);
        TOption option = new();
        m_Options.Add(path, new(GetOptionName(path), path, option));
        return option;
    }

    private void ValidatePath(string path) => m_Validator.Validate(path);

    private IEnumerable<ConfigurationOptionEntry> GetEntriesByPath(string path) => string.IsNullOrEmpty(path) ? Entries : Entries.Where(e => e.Path.StartsWith(path));

    private bool TryGetLoadedSource(out IConfigurationValueSource source) => (source = m_LoadedSource) is not null;

    private bool TryGetNotNullSource(IConfigurationValueSource entrySource, out IConfigurationValueSource source)
    {
        if(entrySource is not null)
        {
            source = m_LoadedSource = entrySource;
            return true;
        }

        return TryGetLoadedSource(out source);
    }

    private readonly struct ConfigurationOptionEntry(string name, string path, IConfigurationOption option)
    {
        internal string Name { get; } = name;

        internal string Path { get; } = path;

        internal IConfigurationOption Option { get; } = option;

        internal void Save(IConfigurationValueSource source) => Option.Save(Path, source);

        internal void Load(IConfigurationValueSource source) => Option.Load(Path, source);
    }

    private class PathValidator(IConfigurationSettings settings)
    {
        private readonly HashSet<string> m_Paths = [];
        private readonly IConfigurationSettings m_Settings = settings;

        internal void Validate(string path)
        {
            ValidateParentPaths(path);
            ValidateChildrenPaths(path);

            m_Paths.Add(path);
        }

        internal void Clear() => m_Paths.Clear();

        internal void Remove(string path) => _ = m_Paths.Remove(path);

        private void ValidateParentPaths(string path)
        {
            foreach(var parentPath in GetParentPaths(path))
            {
                if(!m_Paths.Contains(parentPath)) continue;
                throw new InvalidOperationException($"Cannot request path '{path}' because parent path '{parentPath}' was already requested");
            }
        }

        private void ValidateChildrenPaths(string path)
        {
            foreach(string childPath in m_Paths.Where(requestedPath => IsDescendantPath(requestedPath, path))) throw new InvalidOperationException($"Cannot request path '{path}' because descendant path '{childPath}' was already requested");
        }

        private IEnumerable<string> GetParentPaths(string key)
        {
            var settings = m_Settings;
            var currentPath = string.Empty;

            foreach(var pathPart in key.AsPathEnumerator(settings.Separator))
            {
                currentPath = settings.CombinePaths(currentPath, pathPart);
                yield return currentPath;
            }
        }

        private bool IsDescendantPath(string potentialDescendant, string potentialAncestor) => potentialDescendant.StartsWith(potentialAncestor + m_Settings.Separator);
    }
}
