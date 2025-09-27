namespace SC.Abstraction;

/// <summary>
/// Represents a configuration option.
/// </summary>
public interface IConfigurationOption
{
    /// <summary>
    /// Gets option absolute path.
    /// </summary>
    string Path { get; }

    /// <summary>
    /// Gets option name.
    /// </summary>
    string Name { get; }

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