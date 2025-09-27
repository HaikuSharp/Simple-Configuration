namespace SC.Abstraction;

/// <summary>
/// Represents settings used for configuration management.
/// </summary>
public interface IConfigurationSettings
{
    /// <summary>
    /// Gets the separator used in configuration paths.
    /// </summary>
    string Separator { get; }

    /// <summary>
    /// Gets the initial capacity for configuration collections.
    /// </summary>
    int InitializeCapacity { get; }
}