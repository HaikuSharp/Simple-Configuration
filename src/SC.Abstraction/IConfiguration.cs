using System.Collections.Generic;
using System.Threading.Tasks;

namespace SC.Abstraction;

public interface IConfiguration : IReadOnlyConfiguration
{
    new IEnumerable<IConfigurationOption> LoadedOptions { get; }

    bool HasOption(string path);

    new IConfigurationOption<T> GetOption<T>(string path);

    IConfigurationOption<T> AddOption<T>(string path, T value);

    void RemoveOption(string path);

    void Save(string path);

    void Load(string path);

    Task SaveAsync(string path);

    Task LoadAsync(string path);
}
