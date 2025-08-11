namespace SC.Abstraction;

public interface IRawProvider
{
    bool HasRaw(string path);

    bool TryGetRaw<T>(string path, out T rawValue);

    T GetRaw<T>(string path);

    // void SetRaw<T>(string path, T rawValue);
}