namespace SC.Abstraction;

/// <summary>
/// Represents a configuration option that holds a single value of specified type.
/// </summary>
/// <typeparam name="TValue">The type of the value held by this option.</typeparam>
public interface IValueConfigurationOption<TValue> : IConfigurationOption
{
    /// <summary>
    /// Gets or sets the value of the configuration option.
    /// </summary>
    TValue Value { get; set; }
}
