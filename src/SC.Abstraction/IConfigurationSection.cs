namespace SC.Abstraction;

/// <summary>
/// Represents a section within a configuration hierarchy that has a specific path.
/// </summary>
public interface IConfigurationSection : IConfiguration, IReadOnlyConfigurationSection;