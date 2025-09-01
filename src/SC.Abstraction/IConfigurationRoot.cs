namespace SC.Abstraction;

/// <summary>
/// Represents the root of a configuration hierarchy with access to the underlying value source.
/// </summary>
public interface IConfigurationRoot : IConfiguration
{
    /// <summary>
    /// Gets or sets the value source used by this configuration root.
    /// </summary>
    IConfigurationValueSource Source { get; set; }
}
