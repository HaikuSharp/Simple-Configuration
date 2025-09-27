using SC.Abstraction;

namespace SC;

/// <summary>
/// Provides default configuration settings.
/// </summary>
public sealed class DefaultConfigurationSettings : IConfigurationSettings
{
    /// <summary>
    /// Gets the default instance of <see cref="DefaultConfigurationSettings"/>.
    /// </summary>
    public static DefaultConfigurationSettings Default => field ??= new();

    /// <inheritdoc/>
    public string Separator => ":";

    /// <inheritdoc/>
    public int InitializeCapacity => 32;
}