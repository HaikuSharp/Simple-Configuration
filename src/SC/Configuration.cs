using SC.Abstraction;
using System.Collections.Generic;
using System.Linq;

namespace SC;

public class Configuration(string name, IConfigurationOptions options, IDictionary<string, string> values) : ConfigurationBase(name, options)
{
    private readonly IDictionary<string, string> m_Values = values;

    public Configuration(string name, IDictionary<string, string> values) : this(name, DefaultConfigurationOptions.Default, values) { }

    public Configuration(string name, IConfigurationOptions options) : this(name, options, new Dictionary<string, string>()) { }

    public Configuration(string name) : this(name, DefaultConfigurationOptions.Default, new Dictionary<string, string>()) { }

    public override bool HasSection(string prefix) => m_Values.Keys.Any(k => k.StartsWith(prefix));

    public override bool HasValue(string fullPath) => m_Values.ContainsKey(fullPath);

    public override string GetValue(string fullPath) => m_Values.TryGetValue(fullPath, out string value) ? value : null;

    public override void SetValue(string fullPath, string value) => m_Values[fullPath] = value;

    public override IConfiguration Clone() => new Configuration(Name, Options, m_Values.ToDictionary(k => k.Key, k => k.Value));
}
