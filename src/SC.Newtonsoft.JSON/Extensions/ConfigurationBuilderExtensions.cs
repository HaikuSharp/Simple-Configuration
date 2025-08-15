using SC.Abstraction;

namespace SC.Newtonsoft.JSON.Extensions;

/// <summary>
/// Provides extension methods for <see cref="IConfigurationBuilder"/> to work with JSON files.
/// </summary>
public static class ConfigurationBuilderExtensions
{
    /// <summary>
    /// Appends a JSON file configuration source to the builder.
    /// </summary>
    /// <param name="builder">The configuration builder.</param>
    /// <param name="filePath">The path to the JSON file.</param>
    /// <returns>The configuration builder for chaining.</returns>
    public static IConfigurationBuilder AppendJsonFile(this IConfigurationBuilder builder, string filePath) => builder.Append(new JsonFileConfigurationSource(filePath));
}