using SC.Abstraction;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SC;

/// <summary>
/// Represents a configuration source that loads from and saves to other sources.
/// </summary>
public class CompositeConfigurationValueSource(IEnumerable<IConfigurationValueSource> sources) : IConfigurationValueSource
{
    /// <inheritdoc/>
    public T GetRaw<T>(string path) => sources.FirstOrDefault(s => s.HasRaw(path)).GetRaw<T>(path);

    /// <inheritdoc/>
    public IEnumerable<string> GetRawsNames(string path) => sources.SelectMany(s => s.GetRawsNames(path));

    /// <inheritdoc/>
    public bool HasRaw(string path) => sources.Any(s => s.HasRaw(path));

    /// <inheritdoc/>
    public void RemoveRaw(string path)
    {
        foreach(var source in sources) source.RemoveRaw(path);
    }

    /// <inheritdoc/>
    public void SetRaw<T>(string path, T raw) => sources.FirstOrDefault(s => s.HasRaw(path)).SetRaw(path, raw);

    /// <inheritdoc/>
    public bool TryGetRaw<T>(string path, out T raw)
    {
        foreach(var source in sources) if(source.TryGetRaw(path, out raw)) return true;
        raw = default;
        return false;
    }

    /// <inheritdoc/>
    public void Load()
    {
        foreach(var source in sources) source.Load();
    }

    /// <inheritdoc/>
    public async Task LoadAsync()
    {
        foreach(var source in sources) await source.LoadAsync();
    }

    /// <inheritdoc/>
    public void Save()
    {
        foreach(var source in sources) source.Save();
    }

    /// <inheritdoc/>
    public async Task SaveAsync()
    {
        foreach(var source in sources) await source.SaveAsync();
    }
}