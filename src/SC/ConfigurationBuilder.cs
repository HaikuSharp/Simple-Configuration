using SC.Abstraction;
using System.Collections.Generic;
using System.Linq;

namespace SC;

public class ConfigurationBuilder : IConfigurationBuilder
{
    private readonly List<IConfigurationSource> m_Sources = [];

    public IConfiguration Build(string name, IConfigurationOptions options) => new MultiConfiguration(name, options, m_Sources.Select(s => s.Create(options)));

    public IConfigurationBuilder AppendSource(IConfigurationSource source)
    {
        m_Sources.Add(source);
        return this;
    }
}
