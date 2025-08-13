using System.Collections.Generic;
using System.Threading.Tasks;

namespace SC.Abstraction;

public interface IConfiguration
{
    string Name { get; }

    IConfigurationSettings Settings { get; }

    IEnumerable<IConfigurationOption> LoadedOptions { get; }

    bool HasOption(string path);

    IConfigurationOption<T> GetOption<T>(string path);

    IConfigurationOption<T> AddOption<T>(string path, T value);

    void RemoveOption(string path);

    void Save(string path);

    void Load(string path);

    Task SaveAsync(string path);

    Task LoadAsync(string path);
}
