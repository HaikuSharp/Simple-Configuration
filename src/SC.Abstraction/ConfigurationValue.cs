using System;
using System.ComponentModel;

namespace SC.Abstraction;

public readonly struct ConfigurationValue(string value) : IEquatable<ConfigurationValue>
{
    public const string NULL_VALUE_STRING = "null";

    private readonly string m_Value = value;

    public ConfigurationValue() : this(NULL_VALUE_STRING) { }

    public bool IsNull => m_Value is null or NULL_VALUE_STRING;

    public bool IsEmpty => string.IsNullOrWhiteSpace(m_Value);

    public bool Equals(ConfigurationValue other) => m_Value == other.m_Value;

    public override bool Equals(object obj) => (obj is null && IsNull) || (obj is ConfigurationValue cvalue && Equals(cvalue));

    public override int GetHashCode() => m_Value.GetHashCode();

    public override string ToString() => m_Value;

    public T To<T>() => (T)To(typeof(T));

    public object To(Type type) => type == typeof(byte[]) ? GetBits(m_Value) : To(TypeDescriptor.GetConverter(GetObjectType(type)));

    public object To(TypeConverter converter)
    {
        string value = m_Value;
        return value is null ? default : converter.CanConvertFrom(typeof(string)) ? converter.ConvertFromInvariantString(value) : null;
    }

    private static byte[] GetBits(string base64) => base64 == string.Empty ? [] : Convert.FromBase64String(base64);

    private static Type GetObjectType(Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(type) : type;

    public static implicit operator ConfigurationValue(string value) => new(value);

    public static implicit operator string(ConfigurationValue cvalue) => cvalue.m_Value;

    public static bool operator ==(ConfigurationValue left, ConfigurationValue right) => left.Equals(right);

    public static bool operator !=(ConfigurationValue left, ConfigurationValue right) => !(left == right);
}