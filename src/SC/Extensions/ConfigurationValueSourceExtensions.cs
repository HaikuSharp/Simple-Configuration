using SC.Abstraction;

namespace SC.Extensions;

/// <summary>
/// Provides extension methods for <see cref="IConfigurationValueSource"/>.
/// </summary>
public static class ConfigurationValueSourceExtensions
{
    /// <summary>
    /// Removes all raw values.
    /// </summary>
    public static void Clear(this IConfigurationValueSource source) => source.RemoveRaw(null);
}
