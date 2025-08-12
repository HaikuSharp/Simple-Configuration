using SC.Abstraction;

namespace SC;

public abstract class ConfigurationSourceBase(string name) : IConfigurationSource
{
    public IConfiguration CreateConfiguration(IConfigurationSettings settings) => new Configuration(name, GetValueSource(settings), settings);

    protected abstract IConfigurationValueSource GetValueSource(IConfigurationSettings settings);
}
