namespace SC.Abstraction;

public interface IConfigurationSource
{
    IConfiguration CreateConfiguration(IConfigurationSettings settings);
}
