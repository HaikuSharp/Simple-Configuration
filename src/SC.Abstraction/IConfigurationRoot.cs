namespace SC.Abstraction;

public interface IConfigurationRoot : IConfiguration
{
    bool HasConfiguration(string name);

    IConfiguration GetConfiguration(string name);

    void AddConfiguration(IConfiguration configuration);

    void RemoveConfiguration(string name);
}
