using SC.Abstraction;
using System.Collections;
using System.Collections.Generic;

namespace SC.Options;

/// <summary>
/// Represents a typed list configuration option.
/// </summary>
public sealed class ListConfigurationOption<TItem> : IConfigurationOption, IList<TItem>
{
    private readonly List<TItem> m_List = [];

    /// <inheritdoc/>
    public int Count => m_List.Count;

    /// <inheritdoc/>
    public bool IsReadOnly => false;

    /// <inheritdoc/>
    public TItem this[int index] 
    { 
        get => m_List[index];
        set => m_List[index] = value; 
    }

    /// <inheritdoc/>
    public int IndexOf(TItem item) => m_List.IndexOf(item);

    /// <inheritdoc/>
    public void Insert(int index, TItem item) => m_List.Insert(index, item);

    /// <inheritdoc/>
    public void RemoveAt(int index) => m_List.RemoveAt(index);

    /// <inheritdoc/>
    public void Add(TItem item) => m_List.Add(item);

    /// <inheritdoc/>
    public void Clear() => m_List.Clear();

    /// <inheritdoc/>
    public bool Contains(TItem item) => m_List.Contains(item);

    /// <inheritdoc/>
    public void CopyTo(TItem[] array, int arrayIndex) => m_List.CopyTo(array, arrayIndex);

    /// <inheritdoc/>
    public bool Remove(TItem item) => m_List.Remove(item);

    /// <inheritdoc/>
    public List<TItem>.Enumerator GetEnumerator() => m_List.GetEnumerator();

    /// <inheritdoc/>
    public void Save(string path, IConfigurationValueSource valueSource) => valueSource.SetRaw(path, m_List);

    /// <inheritdoc/>
    public void Load(string path, IConfigurationValueSource valueSource)
    {
        if(!valueSource.TryGetRaw<List<TItem>>(path, out var rawList)) return;

        var list = m_List;

        list.Clear();
        list.AddRange(rawList);
    }

    /// <inheritdoc/>
    IEnumerator<TItem> IEnumerable<TItem>.GetEnumerator() => GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}