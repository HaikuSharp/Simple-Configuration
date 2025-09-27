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
    private readonly Dictionary<string, ConfigurationOptionBase> m_Options = new(settings.InitializeCapacity);

    private IConfigurationValueSource m_LoadedSource;

    /// <inheritdoc/>
    public IConfigurationSettings Settings => settings;

    /// <inheritdoc/>
    public bool HasLoadedSource => m_LoadedSource is not null;

    /// <inheritdoc/>
    public bool HasOption(string path) => m_Options.ContainsKey(path) || (TryGetLoadedSource(out var source) && source.HasRaw(path));

    /// <inheritdoc/>
    public IEnumerable<string> GetOptionsNames(string path)
    {
        var names = m_Options.Values.Where(e => e.Path.StartsWith(path)).Select(e => e.Name);
        if(TryGetLoadedSource(out var source)) names = names.Concat(source.GetRawsNames(path)).Distinct();
        return names;
    }

    /// <inheritdoc/>
    public TOption GetOption<TOption>(string path) where TOption : ConfigurationOptionBase, new() => m_Options.TryGetValue(path, out var option) ? option as TOption : TryGetLoadedSource(out var source) && source.HasRaw(path) ? InternalAddOptionAndLoad<TOption>(path, source) : null;

    /// <inheritdoc/>
    public TOption AddOption<TOption>(string path) where TOption : ConfigurationOptionBase, new() => HasOption(path) ? throw new InvalidOperationException() : InternalAddOption<TOption>(path);

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

        foreach(var entry in GetOptionsByPath(path)) entry.Save(source);
    }

    /// <inheritdoc/>
    public void Load(string path, IConfigurationValueSource source)
    {
        if(!TryGetNotNullSource(source, out source)) return;

        foreach(var entry in GetOptionsByPath(path)) entry.Load(source);
    }

    private TOption InternalAddOptionAndLoad<TOption>(string path, IConfigurationValueSource source) where TOption : ConfigurationOptionBase, new()
    {
        TOption option = InternalAddOption<TOption>(path);
        option.Load(source);
        return option;
    }

    private TOption InternalAddOption<TOption>(string path) where TOption : ConfigurationOptionBase, new()
    {
        ValidatePath(path);
        TOption option = ConfigurationOptionBase.Create<TOption>(path, path.GetOptionName(Settings.Separator));
        m_Options.Add(path, option);
        return option;
    }

    private void ValidatePath(string path) => m_Validator.Validate(path);

    private IEnumerable<ConfigurationOptionBase> GetOptionsByPath(string path) => string.IsNullOrEmpty(path) ? m_Options.Values : m_Options.Values.Where(o => o.Path.StartsWith(path));

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
