namespace SC.Abstraction;

/// <summary>
/// Represents a typed mutable configuration option.
/// </summary>
/// <typeparam name="T">The type of the option value.</typeparam>
public interface IConfigurationOption<T> : IConfigurationOption, IReadOnlyConfigurationOption<T>
{
    /// <summary>
    /// Gets or sets the value of the configuration option.
    /// </summary>
    new T Value { get; set; }
}

/// <summary>
/// Represents a mutable configuration option.
/// </summary>
public interface IConfigurationOption : IReadOnlyConfigurationOption
{
    /// <summary>
    /// Gets or sets the value of the configuration option.
    /// </summary>
    new object Value { get; set; }

    /// <summary>
    /// Saves the option value to the specified value source.
    /// </summary>
    /// <param name="valueSource">The value source to save to.</param>
    void Save(IConfigurationValueSource valueSource);

    /// <summary>
    /// Loads the option value from the specified value source.
    /// </summary>
    /// <param name="valueSource">The value source to load from.</param>
    void Load(IConfigurationValueSource valueSource);
}