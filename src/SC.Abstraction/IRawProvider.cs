using System;

namespace SC.Abstraction;

public interface IRawProvider
{
    bool HasRaw(string path);

    bool TryGetRaw(string path, Type type, out object rawValue);

    object GetRaw(string path, Type type);

    void SetRaw(string path, object rawValue);
}