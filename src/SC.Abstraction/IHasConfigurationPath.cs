namespace SC.Abstraction;

/// <summary>
/// Represents an object that has a configuration path.
/// </summary>
public interface IHasConfigurationPath
{
    /// <summary>
    /// Gets the configuration path.
    /// </summary>
    string Path { get; }
}