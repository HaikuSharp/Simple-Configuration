using SC.Abstraction;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SC;

public class FlattenConfiguration(string name, IConfigurationOptions options, IDictionary<ConfigurationPath, ConfigurationValue> values) : ConfigurationBase(name, options)
{
    private readonly IReadOnlyDictionary<ConfigurationPath, ConfigurationValue> m_ReadOnlyValues = new ReadOnlyDictionary<ConfigurationPath, ConfigurationValue>(values);
    private readonly IDictionary<ConfigurationPath, ConfigurationValue> m_Values = values;

    public FlattenConfiguration(string name, IDictionary<ConfigurationPath, ConfigurationValue> values) : this(name, DefaultConfigurationOptions.Default, values) { }

    public FlattenConfiguration(string name, IEnumerable<ConfigurationPathValuePair> pairs) : this(name, DefaultConfigurationOptions.Default, pairs.ToDictionary(p => p.Path, p => p.Value)) { }

    public FlattenConfiguration(string name, IConfigurationOptions options, IEnumerable<ConfigurationPathValuePair> pairs) : this(name, options, pairs.ToDictionary(p => p.Path, p => p.Value)) { }

    public FlattenConfiguration(string name, IConfigurationOptions options) : this(name, options, new Dictionary<ConfigurationPath, ConfigurationValue>()) { }

    public FlattenConfiguration(string name) : this(name, DefaultConfigurationOptions.Default, new Dictionary<ConfigurationPath, ConfigurationValue>()) { }

    public override IEnumerable<ConfigurationPathValuePair> Pairs => m_ReadOnlyValues.Select(ConfigurationPathValuePair.FromKvp);

    public override IEnumerable<ConfigurationPath> Paths => m_ReadOnlyValues.Keys;

    public override IEnumerable<ConfigurationValue> Values => m_ReadOnlyValues.Values;

    public override bool HasSection(ConfigurationPath prefix) => m_Values.Keys.Any(k => k.IsPrefix(prefix));

    public override bool HasValue(ConfigurationPath fullPath) => m_Values.ContainsKey(fullPath);

    public override ConfigurationValue GetValue(ConfigurationPath fullPath) => m_Values.TryGetValue(fullPath, out var value) ? value : null;

    public override void SetValue(ConfigurationPath fullPath, ConfigurationValue value) => m_Values[fullPath] = value;

    public override IConfiguration Clone() => new FlattenConfiguration(Name, Options, m_Values.ToDictionary(k => k.Key, k => k.Value));

    public static FlattenConfiguration Flat(IConfiguration configuration) => configuration is FlattenConfiguration flattenConfiguration ? flattenConfiguration : new(configuration.Name, configuration.Options, configuration.Pairs);
}
