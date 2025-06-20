using SC.Abstraction;
using System.Collections.Generic;
using System.Linq;

namespace SC;

public class ConfigurationBuilder : IConfigurationBuilder
{
    private readonly List<IConfigurationSource> m_Sources = [];

    public IConfigurationSection Build() => new ConfigurationSection("root", m_Sources.Select(s => s.CreateSection()).ToDictionary(s => s.Name, s => (object)s));

    public IConfigurationBuilder AppendSource(IConfigurationSource source)
    {
        m_Sources.Add(source);
        return this;
    }
}
