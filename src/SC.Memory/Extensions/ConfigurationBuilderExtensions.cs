using SC.Abstraction;
using System.Collections.Generic;

namespace SC.Memory.Extensions;

/// <summary>
/// Provides extension methods for <see cref="IConfigurationBuilder"/> to work with in-memory configurations.
/// </summary>
public static class ConfigurationBuilderExtensions
{
    /// <summary>
    /// Appends an in-memory configuration source to the builder.
    /// </summary>
    /// <param name="builder">The configuration builder.</param>
    /// <param name="name">The name of the configuration.</param>
    /// <param name="source">The dictionary containing configuration data.</param>
    /// <returns>The configuration builder for chaining.</returns>
    public static IConfigurationBuilder AppendMemory(this IConfigurationBuilder builder, string name, IDictionary<string, object> source) => builder.Append(new MemoryConfigurationSource(name, source));
}