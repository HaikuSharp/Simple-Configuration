using SC.Abstraction;

namespace SC;

public abstract class ConfigurationSourceBase(string name) : IConfigurationSource
{
    public IConfiguration CreateConfiguration(IConfigurationSettings settings) => new Configuration(name, GetRawProvider(settings), settings);

    protected abstract IRawProvider GetRawProvider(IConfigurationSettings settings);
}
