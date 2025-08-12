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

        public bool TryGetRaw(string path, Type type, out object rawValue)
        {
            if(source.TryGetValue(path, out object value))
            {
                rawValue = InternalConvertValue(value, type);
                return true;
            }

            rawValue = default;
            return false;
        }

        public object GetRaw(string path, Type type) => InternalConvertValue(source[path], type);

        public void SetRaw(string path, object rawValue) => source[path] = rawValue;

        private static object InternalConvertValue(object sourceValue, Type type)
        {
            var converter = TypeDescriptor.GetConverter(sourceValue.GetType());
            return converter.CanConvertTo(type) ? converter.ConvertTo(sourceValue, type) : throw new InvalidCastException();
        }
    }
}
