using SC.Abstraction;
using SC.Extensions;

namespace SC;

public class ConfigurationSection(IConfiguration configuration, string path) : IConfigurationSection
{
    public string Name { get; } = string.Format(configuration.Settings.SectionNameFormat, configuration.Name, path);

    public string Path => path;

    public IConfigurationSettings Settings => configuration.Settings;

    public bool HasOption(string path) => configuration.HasOption(GetAbsolutePath(path));

    public IConfigurationOption<T> GetOption<T>(string path) => configuration.GetOption<T>(GetAbsolutePath(path));

    public IConfigurationOption<T> AddOption<T>(string path, T value) => configuration.AddOption(GetAbsolutePath(path), value);

    public string GetAbsolutePath(string path) => Settings.CombinePaths(Path, path);
}
