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
    public IEnumerable<IConfigurationOption> Options => configuration.Options.Where(o => o.Path.StartsWith(Path));

    /// <inheritdoc/>
    IEnumerable<IReadOnlyConfigurationOption> IReadOnlyConfiguration.Options => Options;

    /// <inheritdoc/>
    public bool HasOption(string path) => configuration.HasOption(GetAbsolutePath(path));

    /// <inheritdoc/>
    public IEnumerable<string> GetOptionsNames(string path) => throw new System.NotImplementedException();

    /// <inheritdoc/>
    public IConfigurationOption<T> GetOption<T>(string path) => configuration.GetOption<T>(GetAbsolutePath(path));

    /// <inheritdoc/>
    public IConfigurationOption<T> AddOption<T>(string path, T value) => configuration.AddOption(GetAbsolutePath(path), value);

    /// <inheritdoc/>
    public void RemoveOption(string path) => configuration.RemoveOption(GetAbsolutePath(path));

    /// <inheritdoc/>
    public string GetAbsolutePath(string path) => Settings.CombinePaths(Path, path);

    /// <inheritdoc/>
    public void Save(string path, IConfigurationValueSource source) => configuration.Save(GetAbsolutePath(path), source);

    /// <inheritdoc/>
    public void Load(string path, IConfigurationValueSource source) => configuration.Load(GetAbsolutePath(path), source);

    /// <inheritdoc/>
    public Task SaveAsync(string path, IConfigurationValueSource source) => configuration.SaveAsync(GetAbsolutePath(path), source);

    /// <inheritdoc/>
    public Task LoadAsync(string path, IConfigurationValueSource source) => configuration.LoadAsync(GetAbsolutePath(path), source);

    /// <inheritdoc/>
    IReadOnlyConfigurationOption<T> IReadOnlyConfiguration.GetOption<T>(string path) => GetOption<T>(path);
}