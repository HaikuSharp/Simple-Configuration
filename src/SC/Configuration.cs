using SC.Abstraction;
using SC.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SC;

/// <inheritdoc cref="IConfiguration"/>
public sealed class Configuration(IConfigurationSettings settings) : IConfigurationRoot
{
    private readonly PathValidator m_Validator = new(settings);
    private readonly Dictionary<string, IConfigurationOption> m_Options = new(settings.InitializeCapacity);

    private IConfigurationValueSource m_LoadedSource;

    /// <inheritdoc/>
    public event ConfigurationUpdateHandler OnLoaded;

    /// <inheritdoc/>
    public event ConfigurationUpdateHandler OnSaved;

    /// <inheritdoc/>
    public IConfigurationSettings Settings => settings;

    /// <inheritdoc/>
    public bool HasLoadedSource => m_LoadedSource is not null;

    /// <inheritdoc/>
    public bool HasOption(string path) => m_Options.ContainsKey(path) || (TryGetLoadedSource(out var source) && source.HasRaw(path));

    /// <inheritdoc/>
    public IEnumerable<string> GetOptionsNames(string path)
    {
        var names = GetOptionsByPath(path).Select(e => e.Key.GetSectionName(path, Settings.Separator));
        if(TryGetLoadedSource(out var source)) names = names.Concat(source.GetRawsNames(path)).Distinct();
        return names;
    }

    /// <inheritdoc/>
    public TOption GetOption<TOption>(string path) where TOption : class, IConfigurationOption, new() => m_Options.TryGetValue(path, out var option) ? option as TOption : TryGetLoadedSource(out var source) && source.HasRaw(path) ? InternalAddOptionAndLoad<TOption>(path, source) : null;

    /// <inheritdoc/>
    public TOption AddOption<TOption>(string path) where TOption : class, IConfigurationOption, new() => HasOption(path) ? throw new InvalidOperationException($"Option {path} already added.") : InternalAddOption<TOption>(path);

    /// <inheritdoc/>
    public void RemoveOption(string path)
    {
        foreach(var optionPath in GetOptionsByPath(path).Select(e => e.Key).ToArray())
        {
            if(!m_Options.Remove(optionPath)) continue;
            m_Validator.Remove(optionPath);
        }
    }

    /// <inheritdoc/>
    public void Save(string path, IConfigurationValueSource source)
    {
        if(!TryGetNotNullSource(source, out source)) return;

        foreach(var entry in GetOptionsByPath(path)) entry.Value.Save(entry.Key, source);

        OnSaved?.Invoke(path);
    }

    /// <inheritdoc/>
    public void Load(string path, IConfigurationValueSource source)
    {
        if(!TryGetNotNullSource(source, out source)) return;

        foreach(var entry in GetOptionsByPath(path)) entry.Value.Load(entry.Key, source);

        OnLoaded?.Invoke(path);
    }

    private TOption InternalAddOptionAndLoad<TOption>(string path, IConfigurationValueSource source) where TOption : class, IConfigurationOption, new()
    {
        TOption option = InternalAddOption<TOption>(path);
        option.Load(path, source);
        return option;
    }

    private TOption InternalAddOption<TOption>(string path) where TOption : class, IConfigurationOption, new()
    {
        ValidatePath(path);
        TOption option = new();
        m_Options.Add(path, option);
        return option;
    }

    private void ValidatePath(string path) => m_Validator.Validate(path);

    private IEnumerable<KeyValuePair<string, IConfigurationOption>> GetOptionsByPath(string path) => string.IsNullOrEmpty(path) ? m_Options : m_Options.Where(kvp => m_Validator.IsDescendantPath(kvp.Key, path));

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

        internal bool IsDescendantPath(string potentialDescendant, string potentialAncestor) => potentialDescendant.StartsWith(potentialAncestor + m_Settings.Separator);

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
    }
}
