using SC.Abstraction;
using System.Collections;
using System.Collections.Generic;

namespace SC.Options;

/// <summary>
/// Represents a typed dictionary configuration option.
/// </summary>
public sealed class DictionaryConfigurationOption<TKey, TItem> : ConfigurationOptionBase, IDictionaryConfigurationOption<TKey, TItem>
{
    private readonly Dictionary<TKey, TItem> m_Dictionary = [];

    /// <inheritdoc/>
    public ICollection<TKey> Keys => m_Dictionary.Keys;

    /// <inheritdoc/>
    public ICollection<TItem> Values => m_Dictionary.Values;

    /// <inheritdoc/>
    public int Count => m_Dictionary.Count;

    /// <inheritdoc/>
    public bool IsReadOnly => false;

    /// <inheritdoc/>
    public TItem this[TKey key] 
    { 
        get => m_Dictionary[key]; 
        set => m_Dictionary[key] = value; 
    }

    /// <inheritdoc/>
    public void Add(TKey key, TItem value) => m_Dictionary.Add(key, value);

    /// <inheritdoc/>
    public bool ContainsKey(TKey key) => m_Dictionary.ContainsKey(key);

    /// <inheritdoc/>
    public bool Remove(TKey key) => m_Dictionary.Remove(key);

    /// <inheritdoc/>
    public bool TryGetValue(TKey key, out TItem value) => m_Dictionary.TryGetValue(key, out value);

    /// <inheritdoc/>
    public void Add(KeyValuePair<TKey, TItem> item) => ((ICollection<KeyValuePair<TKey, TItem>>)m_Dictionary).Add(item);

    /// <inheritdoc/>
    public void Clear() => m_Dictionary.Clear();

    /// <inheritdoc/>
    public bool Contains(KeyValuePair<TKey, TItem> item) => ((ICollection<KeyValuePair<TKey, TItem>>)m_Dictionary).Contains(item);

    /// <inheritdoc/>
    public void CopyTo(KeyValuePair<TKey, TItem>[] array, int arrayIndex) => ((ICollection<KeyValuePair<TKey, TItem>>)m_Dictionary).CopyTo(array, arrayIndex);

    /// <inheritdoc/>
    public bool Remove(KeyValuePair<TKey, TItem> item) => ((ICollection<KeyValuePair<TKey, TItem>>)m_Dictionary).Remove(item);

    /// <inheritdoc/>
    protected override void Save(string path, IConfigurationValueSource valueSource) => valueSource.SetRaw(path, m_Dictionary);

    /// <inheritdoc/>
    protected override void Load(string path, IConfigurationValueSource valueSource)
    {
        if(!valueSource.TryGetRaw<Dictionary<TKey, TItem>>(path, out var rawDictionary)) return;

        var dictionary = m_Dictionary;

        dictionary.Clear();
        foreach(var kvp in rawDictionary) dictionary.Add(kvp.Key, kvp.Value);
    }

    /// <inheritdoc/>
    public Dictionary<TKey, TItem>.Enumerator GetEnumerator() => m_Dictionary.GetEnumerator();

    IEnumerator<KeyValuePair<TKey, TItem>> IEnumerable<KeyValuePair<TKey, TItem>>.GetEnumerator() => GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
