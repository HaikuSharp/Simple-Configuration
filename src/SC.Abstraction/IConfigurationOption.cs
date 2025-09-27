namespace SC.Abstraction;

/// <summary>
/// Represents a configuration option.
/// </summary>
public interface IConfigurationOption
{
    /// <summary>
    /// Saves the option value to the specified value source.
    /// </summary>
    /// <param name="path">The source path.</param>
    /// <param name="valueSource">The value source to save to.</param>
    void Save(string path, IConfigurationValueSource valueSource);

    /// <summary>
    /// Loads the option value from the specified value source.
    /// </summary>
    /// <param name="path">The source path.</param>
    /// <param name="valueSource">The value source to load from.</param>
    void Load(string path, IConfigurationValueSource valueSource);
}