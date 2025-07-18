namespace SC.Abstraction;

public interface IConfigurationSection : IConfiguration
{
    ConfigurationPath Prefix { get; }

    ConfigurationPath GetAbsolutePath(ConfigurationPath pathPart);
}
