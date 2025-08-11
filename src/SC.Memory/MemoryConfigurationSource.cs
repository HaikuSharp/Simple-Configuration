using SC;
using SC.Abstraction;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SC.Memory;

public class MemoryConfigurationSource(string name, IDictionary<string, object> source) : ConfigurationSourceBase(name)
{
    protected override IRawProvider GetRawProvider(IConfigurationSettings settings) => new MemoryRawProvider(source);

    private class MemoryRawProvider(IDictionary<string, object> source) : IRawProvider
    {
        public bool HasRaw(string path) => source.ContainsKey(path);

        public bool TryGetRaw<T>(string path, out T rawValue)
        {
            if(source.TryGetValue(path, out var value))
            {
                rawValue = InternalConvertValue<T>(value);
                return true;
            }

            rawValue = default;
            return false;
        }

        public T GetRaw<T>(string path) => InternalConvertValue<T>(source[path]);

        private static T InternalConvertValue<T>(object sourceValue)
        {
            var converter = TypeDescriptor.GetConverter(sourceValue.GetType());
            return converter.CanConvertTo(typeof(T)) ? (T)converter.ConvertTo(sourceValue, typeof(T)) : throw new InvalidCastException();
        }
    }
}
