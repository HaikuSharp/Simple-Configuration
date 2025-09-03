namespace SC.Abstraction;

/// <summary>
/// Represents a source for configuration values that supports reading and writing from file.
/// </summary>
public interface IFileConfigurationValueSource : IConfigurationValueSource
{
    /// <summary>
    /// Gets the file path.
    /// </summary>
    string FilePath { get; }
}
