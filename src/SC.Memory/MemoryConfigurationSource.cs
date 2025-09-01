using SC.Abstraction;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace SC.Memory;

/// <summary>
/// Represents a configuration source that stores values in memory.
/// </summary>
public class MemoryConfigurationSource(string name, IDictionary<string, object> source) : ConfigurationSourceBase(name)
{
    /// <inheritdoc/>
    protected override IConfigurationValueSource GetValueSource(IConfigurationSettings settings) => new MemoryConfigurationValueSource(source);

    private class MemoryConfigurationValueSource(IDictionary<string, object> source) : IConfigurationValueSource
    {
        private readonly IDictionary<string, object> m_Source = source;
        private readonly Dictionary<string, object> m_Values = source.ToDictionary(k => k.Key, k => k.Value);

        /// <inheritdoc/>
        public bool HasRaw(string path) => m_Values.ContainsKey(path);

        /// <inheritdoc/>
        public IEnumerable<string> GetRawsNames(string path) => m_Values.Keys.Where(p => p.StartsWith(path));

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
        public void RemoveRaw(string path) => _ = m_Values.Remove(path);

        private static T InternalConvertValue<T>(object sourceValue) => (T)InternalConvertValue(sourceValue, typeof(T));

        private static object InternalConvertValue(object sourceValue, Type type)
        {
            var converter = TypeDescriptor.GetConverter(sourceValue.GetType());
            return converter.CanConvertTo(type) ? converter.ConvertTo(sourceValue, type) : throw new InvalidCastException();
        }

        /// <inheritdoc/>
        public void Save() => CopyValues(m_Source, m_Values);

        /// <inheritdoc/>
        public void Load() => CopyValues(m_Values, m_Source);

        private static void CopyValues(IDictionary<string, object> values, IDictionary<string, object> source)
        {
            values.Clear();
            foreach(var kvp in source) values[kvp.Key] = kvp.Value;
        }

        /// <inheritdoc/>
        public Task SaveAsync()
        {
            Save();
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task LoadAsync()
        {
            Load();
            return Task.CompletedTask;
        }
    }
}