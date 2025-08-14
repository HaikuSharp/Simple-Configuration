using System.Collections.Generic;

namespace SC.Abstraction;

public interface IConfigurationRoot : IConfiguration
{
    IEnumerable<IConfiguration> LoadedConfigurations { get; }

    bool HasConfiguration(string name);

    IConfiguration GetConfiguration(string name);

    void AddConfiguration(IConfiguration configuration);

    void RemoveConfiguration(string name);
}
