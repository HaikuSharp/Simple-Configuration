using SC.Abstraction;

namespace SC.Extensions;

/// <summary>
/// Provides extension methods for string operations related to configuration paths.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Extracts a section name from a configuration path using the specified prefix and separator.
    /// </summary>
    /// <param name="path">The configuration path to extract from.</param>
    /// <param name="prefix">The prefix to skip in the path.</param>
    /// <param name="separator">The separator used in the configuration path.</param>
    /// <returns>The extracted section name, or the original path if no section is found.</returns>
    public static string GetSectionName(this string path, string prefix, string separator)
    {
        if(string.IsNullOrEmpty(path)) return prefix;

        int startSectionNameIndex = path.IndexOf(separator, prefix.Length);

        if(startSectionNameIndex is -1) return path;

        startSectionNameIndex += separator.Length;
        int endSectionNameIndex = path.IndexOf(separator, startSectionNameIndex);

        if(endSectionNameIndex is -1) endSectionNameIndex = path.Length - 1;

#pragma warning disable IDE0079
#pragma warning disable IDE0057

        return path.Substring(startSectionNameIndex, endSectionNameIndex - startSectionNameIndex);

#pragma warning restore IDE0057
#pragma warning restore IDE0079
    }

    /// <summary>
    /// Converts a string path to a configuration path enumerator using the specified separator.
    /// </summary>
    /// <param name="path">The configuration path to enumerate.</param>
    /// <param name="separator">The separator used to split the path.</param>
    /// <returns>A <see cref="ConfigurationPathEnumerator"/> for iterating through path segments.</returns>
    public static ConfigurationPathEnumerator AsPathEnumerator(this string path, string separator) => path.AsPathEnumerator(separator);
}