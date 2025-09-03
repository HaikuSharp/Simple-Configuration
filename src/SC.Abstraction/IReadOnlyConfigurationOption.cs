using System;

namespace SC.Abstraction;

/// <summary>
/// Represents a typed read-only configuration option.
/// </summary>
/// <typeparam name="T">The type of the option value.</typeparam>
public interface IReadOnlyConfigurationOption<T> : IReadOnlyConfigurationOption
{
    /// <summary>
    /// Gets the value of the configuration option.
    /// </summary>
    new T Value { get; }
}

/// <summary>
/// Represents a read-only configuration option.
/// </summary>
public interface IReadOnlyConfigurationOption
{
    /// <summary>
    /// Gets the name of the configuration option.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the path of the configuration option.
    /// </summary>
    string Path { get; }

    /// <summary>
    /// Gets the type of the option value.
    /// </summary>
    Type ValueType { get; }

    /// <summary>
    /// Gets the value of the configuration option.
    /// </summary>
    object Value { get; }
}