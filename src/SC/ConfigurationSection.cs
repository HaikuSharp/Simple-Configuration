using SC.Abstraction;
using SC.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SC;

/// <inheritdoc cref="IConfigurationSection"/>
public sealed class ConfigurationSection(IConfiguration configuration, string path) : IConfigurationSection
{
    /// <inheritdoc/>
    public string Name { get; } = string.Format(configuration.Settings.SectionNameFormat, configuration.Name, path);

    /// <inheritdoc/>
    public string Path => path;

    /// <inheritdoc/>
    public IConfigurationSettings Settings => configuration.Settings;

    /// <inheritdoc/>
    public IEnumerable<IConfigurationOption> LoadedOptions => configuration.LoadedOptions.Where(o => o.Path.StartsWith(Path));

    /// <inheritdoc/>
    IEnumerable<IReadOnlyConfigurationOption> IReadOnlyConfiguration.LoadedOptions => LoadedOptions;

    /// <inheritdoc/>
    public bool HasOption(string path) => configuration.HasOption(GetAbsolutePath(path));

    /// <inheritdoc/>
    public IConfigurationOption<T> GetOption<T>(string path) => configuration.GetOption<T>(GetAbsolutePath(path));

    /// <inheritdoc/>
    public IConfigurationOption<T> AddOption<T>(string path, T value) => configuration.AddOption(GetAbsolutePath(path), value);

    /// <inheritdoc/>
    public void RemoveOption(string path) => configuration.RemoveOption(GetAbsolutePath(path));

    /// <inheritdoc/>
    public string GetAbsolutePath(string path) => Settings.CombinePaths(Path, path);

    /// <inheritdoc/>
    public void Save(string path) => configuration.Save(GetAbsolutePath(path));

    /// <inheritdoc/>
    public void Load(string path) => configuration.Load(GetAbsolutePath(path));

    /// <inheritdoc/>
    public async Task SaveAsync(string path) => await configuration.SaveAsync(GetAbsolutePath(path));

    /// <inheritdoc/>
    public async Task LoadAsync(string path) => await configuration.LoadAsync(GetAbsolutePath(path));

    /// <inheritdoc/>
    IReadOnlyConfigurationOption<T> IReadOnlyConfiguration.GetOption<T>(string path) => GetOption<T>(path);
}