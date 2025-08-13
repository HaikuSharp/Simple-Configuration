using System.Threading.Tasks;

namespace SC.Abstraction;

public interface IConfigurationValueSource
{
    bool HasRaw(string path);

    bool TryGetRaw<T>(string path, out T raw);

    T GetRaw<T>(string path);

    void SetRaw<T>(string path, T raw);

    void RemoveRaw(string path);

    void Save();

    void Load();

    Task SaveAsync();

    Task LoadAsync();
}