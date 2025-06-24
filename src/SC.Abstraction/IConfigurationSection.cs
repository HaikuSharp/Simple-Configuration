namespace SC.Abstraction;

public interface IConfigurationSection : IConfiguration
{
    string Prefix { get; }
}
