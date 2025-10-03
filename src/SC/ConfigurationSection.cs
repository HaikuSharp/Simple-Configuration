using SC.Abstraction;
using SC.Extensions;
using System.Collections.Generic;

namespace SC;

/// <inheritdoc cref="IConfigurationSection"/>
public sealed class ConfigurationSection(IConfiguration configuration, string path) : IConfigurationSection
{
    /// <inheritdoc/>
    public string Path => path;

    /// <inheritdoc/>
    public IConfigurationSettings Settings => configuration.Settings;

    /// <inheritdoc/>
    public bool HasLoadedSource => configuration.HasLoadedSource;

    /// <inheritdoc/>
    public bool HasOption(string path) => configuration.HasOption(GetAbsolutePath(path));

    /// <inheritdoc/>
    public IEnumerable<string> GetOptionsNames(string path) => configuration.GetOptionsNames(GetAbsolutePath(path));

    /// <inheritdoc/>
    public TOption GetOption<TOption>(string path) where TOption : class, IConfigurationOption, new() => configuration.GetOption<TOption>(GetAbsolutePath(path));

    /// <inheritdoc/>
    public TOption AddOption<TOption>(string path) where TOption : class, IConfigurationOption, new() => configuration.AddOption<TOption>(GetAbsolutePath(path));

    /// <inheritdoc/>
    public void RemoveOption(string path) => configuration.RemoveOption(GetAbsolutePath(path));

    /// <inheritdoc/>
    public string GetAbsolutePath(string path) => Settings.CombinePaths(Path, path);

    /// <inheritdoc/>
    public void Save(string path, IConfigurationValueSource source) => configuration.Save(GetAbsolutePath(path), source);

    /// <inheritdoc/>
    public void Load(string path, IConfigurationValueSource source) => configuration.Load(GetAbsolutePath(path), source);

    /// <inheritdoc/>
    public void Sync(string path, IConfigurationValueSource source) => configuration.Sync(GetAbsolutePath(path), source);
}