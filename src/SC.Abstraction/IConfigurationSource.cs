namespace SC.Abstraction;

public interface IConfigurationSource
{
    IConfiguration GetConfiguration(IConfigurationSettings settings);
}