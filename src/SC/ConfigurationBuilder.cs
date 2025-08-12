using SC.Abstraction;
using System.Collections.Generic;

namespace SC;

public class ConfigurationBuilder : IConfigurationBuilder
{
    private readonly List<IConfigurationSource> m_Sources = [];

    public IConfigurationRoot Build(string name, IConfigurationSettings settings)
    {
        ConfigurationRoot root = new(name, settings);
        foreach(var source in m_Sources) root.AddConfiguration(source.CreateConfiguration(settings));
        return root;
    }

    public IConfigurationBuilder Append(IConfigurationSource source)
    {
        m_Sources.Add(source);
        return this;
    }
}
