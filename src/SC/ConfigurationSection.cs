using SC.Abstraction;
using SC.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SC;

public class ConfigurationSection(IConfiguration configuration, string path) : IConfigurationSection
{
    public string Name { get; } = string.Format(configuration.Settings.SectionNameFormat, configuration.Name, path);

    public string Path => path;

    public IConfigurationSettings Settings => configuration.Settings;

    public IEnumerable<IConfigurationOption> LoadedOptions => configuration.LoadedOptions.Where(o => o.Path.StartsWith(Path));

    public bool HasOption(string path) => configuration.HasOption(GetAbsolutePath(path));

    public IConfigurationOption<T> GetOption<T>(string path) => configuration.GetOption<T>(GetAbsolutePath(path));

    public IConfigurationOption<T> AddOption<T>(string path, T value) => configuration.AddOption(GetAbsolutePath(path), value);

    public string GetAbsolutePath(string path) => Settings.CombinePaths(Path, path);

    public void Save(string path) => configuration.Save(GetAbsolutePath(path));

    public void Load(string path) => configuration.Load(GetAbsolutePath(path));

    public async Task SaveAsync(string path) => await configuration.SaveAsync(GetAbsolutePath(path));

    public async Task LoadAsync(string path) => await configuration.LoadAsync(GetAbsolutePath(path));
}
