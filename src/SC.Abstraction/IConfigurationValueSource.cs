using System.Collections.Generic;
using System.Threading.Tasks;

namespace SC.Abstraction;

/// <summary>
/// Represents a source for configuration values that supports save and load.
/// </summary>
public interface ILoadableConfigurationValueSource : IConfigurationValueSource
{
    /// <summary>
    /// Saves all changes to the value source.
    /// </summary>
    void Save();

    /// <summary>
    /// Loads all values from the value source.
    /// </summary>
    void Load();

    /// <summary>
    /// Asynchronously saves all changes to the value source.
    /// </summary>
    Task SaveAsync();

    /// <summary>
    /// Asynchronously loads all values from the value source.
    /// </summary>
    Task LoadAsync();
}

/// <summary>
/// Represents a source for configuration values that supports reading and writing.
/// </summary>
public interface IConfigurationValueSource
{
    /// <summary>
    /// Determines whether the source contains a raw value at the specified path.
    /// </summary>
    /// <param name="path">The path of the value to check.</param>
    /// <returns>
    /// <c>true</c> if the source contains a value at the specified path; otherwise, <c>false</c>.
    /// </returns>
    bool HasRaw(string path);

    /// <summary>
    /// Retrieves the names of all raw value entries located at the specified path.
    /// This method is typically used for exploring hierarchical configuration structures
    /// and discovering available configuration keys within a given section.
    /// </summary>
    /// <param name="path">The path to search for raw value entries.</param>
    /// <returns>
    /// An <see cref="IEnumerable{String}"/> containing the names of all raw value entries
    /// found at the specified path. Returns an empty collection if no entries are found.
    /// </returns>
    IEnumerable<string> GetRawsNames(string path);

    /// <summary>
    /// Attempts to get the raw value at the specified path.
    /// </summary>
    /// <typeparam name="T">The type of the value to get.</typeparam>
    /// <param name="path">The path of the value to get.</param>
    /// <param name="raw">When this method returns, contains the value if found; otherwise, the default value.</param>
    /// <returns>
    /// <c>true</c> if the value was found; otherwise, <c>false</c>.
    /// </returns>
    bool TryGetRaw<T>(string path, out T raw);

    /// <summary>
    /// Gets the raw value at the specified path.
    /// </summary>
    /// <typeparam name="T">The type of the value to get.</typeparam>
    /// <param name="path">The path of the value to get.</param>
    /// <returns>The value at the specified path.</returns>
    T GetRaw<T>(string path);

    /// <summary>
    /// Sets the raw value at the specified path.
    /// </summary>
    /// <typeparam name="T">The type of the value to set.</typeparam>
    /// <param name="path">The path where to set the value.</param>
    /// <param name="raw">The value to set.</param>
    void SetRaw<T>(string path, T raw);

    /// <summary>
    /// Removes the raw value at the specified path.
    /// </summary>
    /// <param name="path">The path of the value to remove.</param>
    void RemoveRaw(string path);
}