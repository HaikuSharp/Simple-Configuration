using System;

namespace SC.Abstraction;

public interface IReadOnlyConfigurationOption<T> : IReadOnlyConfigurationOption
{
    new T Value { get; }
}

public interface IReadOnlyConfigurationOption
{
    string Path { get; }

    Type ValueType { get; }

    object Value { get; }
}