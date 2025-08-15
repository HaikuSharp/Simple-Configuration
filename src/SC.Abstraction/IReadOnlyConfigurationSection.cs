namespace SC.Abstraction;

/// <summary>
/// Represents a read-only section within a configuration hierarchy that has a specific path.
/// </summary>
public interface IReadOnlyConfigurationSection : IReadOnlyConfiguration, IHasConfigurationPath
{
    /// <summary>
    /// Gets the absolute path by combining the section's path with the specified relative path.
    /// </summary>
    /// <param name="path">The relative path to combine.</param>
    /// <returns>The absolute path.</returns>
    string GetAbsolutePath(string path);
}