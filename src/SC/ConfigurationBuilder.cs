using SC.Abstraction;
using System.Collections.Generic;

namespace SC;

public class ConfigurationBuilder : IConfigurationBuilder
{
    private readonly List<IConfigurationSource> m_Sources = [];

    public IConfiguration Build(string name, IConfigurationSettings settings)
    {
        MergedConfiguration mergedConfiguration = new(name, settings);
        foreach(var source in m_Sources) mergedConfiguration.AddConfiguration(source.CreateConfiguration(settings));
        return mergedConfiguration;
    }

    public IConfigurationBuilder Append(IConfigurationSource source)
    {
        m_Sources.Add(source);
        return this;
    }
}
