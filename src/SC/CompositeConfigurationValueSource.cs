using SC.Abstraction;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SC;

/// <summary>
/// Represents a configuration values source that loads from and saves to other sources.
/// </summary>
public sealed class CompositeConfigurationValueSource<TSource> : IConfigurationValueSource where TSource : IConfigurationValueSource
{
    private readonly List<TSource> m_Sources = [];

    /// <summary>
    /// Gets readonly sources list.
    /// </summary>
    public IReadOnlyList<TSource> Sources => m_Sources;

    /// <inheritdoc/>
    public T GetRaw<T>(string path)
    {
        var source = m_Sources.FirstOrDefault(s => s.HasRaw(path));
        return source is not null ? source.GetRaw<T>(path) : default;
    }

    /// <inheritdoc/>
    public IEnumerable<string> GetRawsNames(string path) => m_Sources.SelectMany(s => s.GetRawsNames(path)).Distinct();

    /// <inheritdoc/>
    public bool HasRaw(string path) => m_Sources.Any(s => s.HasRaw(path));

    /// <inheritdoc/>
    public void RemoveRaw(string path) => m_Sources.ForEach(s => s.RemoveRaw(path));

    /// <inheritdoc/>
    public void SetRaw<T>(string path, T raw) => m_Sources.FirstOrDefault(s => s.HasRaw(path))?.SetRaw(path, raw);

    /// <inheritdoc/>
    public bool TryGetRaw<T>(string path, out T raw)
    {
        foreach(var source in m_Sources) if(source.TryGetRaw(path, out raw)) return true;
        raw = default;
        return false;
    }

    /// <inheritdoc/>
    public void Clear() => m_Sources.ForEach(s => s.Clear());

    /// <inheritdoc/>
    public void Load() => m_Sources.ForEach(s => s.Load());

    /// <inheritdoc/>
    public async Task LoadAsync()
    {
        foreach(var source in m_Sources) await source.LoadAsync();
    }

    /// <inheritdoc/>
    public void Save() => m_Sources.ForEach(s => s.Save());

    /// <inheritdoc/>
    public async Task SaveAsync()
    {
        foreach(var source in m_Sources) await source.SaveAsync();
    }

    /// <summary>
    /// Append new values source.
    /// </summary>
    /// <param name="source">The source to append.</param>
    public void AppendSource(TSource source) => m_Sources.Add(source);

    /// <summary>
    /// Remove a values source.
    /// </summary>
    /// <param name="source">The source to remove.</param>
    public void RemoveSource(TSource source) => _ = m_Sources.Remove(source);
}