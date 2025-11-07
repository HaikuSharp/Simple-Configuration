using SC.Abstraction;
using SC.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace SC.Memory;

/// <summary>
/// Represents a configuration values source that loads from and saves to in-memory collection.
/// </summary>
public class MemoryConfigurationValueSource(IDictionary<string, object> source, IConfigurationSettings settings) : IConfigurationValueSource
{
    private readonly IDictionary<string, object> m_Values = source;

    /// <inheritdoc/>
    public bool HasRaw(string path) => m_Values.ContainsKey(path);

    /// <inheritdoc/>
    public IEnumerable<string> GetRawsNames(string path) => GetChildrenPaths(path).Select(p => p.GetSectionName(path, settings.Separator));

    /// <inheritdoc/>
    public bool TryGetRaw<T>(string path, out T rawValue)
    {
        if(m_Values.TryGetValue(path, out object value))
        {
            rawValue = InternalConvertValue<T>(value);
            return true;
        }

        rawValue = default;
        return false;
    }

    /// <inheritdoc/>
    public T GetRaw<T>(string path) => InternalConvertValue<T>(m_Values[path]);

    /// <inheritdoc/>
    public void SetRaw<T>(string path, T rawValue) => m_Values[path] = rawValue;

    /// <inheritdoc/>
    public void RemoveRaw(string path)
    {
        if(string.IsNullOrEmpty(path))
        {
            m_Values.Clear();
            return;
        }

        foreach(var childPath in GetChildrenPaths(path).ToArray()) m_Values.Remove(childPath);
    }

    private static T InternalConvertValue<T>(object sourceValue) => (T)InternalConvertValue(sourceValue, typeof(T));

    private static object InternalConvertValue(object sourceValue, Type type)
    {
        var converter = TypeDescriptor.GetConverter(sourceValue.GetType());
        return converter.CanConvertTo(type) ? converter.ConvertTo(sourceValue, type) : throw new InvalidCastException($"Failed to convert object of type {sourceValue.GetType().FullName} to object of type {type.FullName}");
    }

    private IEnumerable<string> GetChildrenPaths(string path) => string.IsNullOrEmpty(path) ? m_Values.Keys : m_Values.Keys.Where(p => p.StartsWith(path));
}
