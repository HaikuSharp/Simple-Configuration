using SC.Abstraction;
using System.Collections.Generic;
using System.Linq;

namespace SC;

public sealed class MultiConfiguration(string name, IConfigurationOptions options, IEnumerable<IConfiguration> configurations) : ConfigurationBase(name, options)
{
    public override IEnumerable<ConfigurationPathValuePair> Pairs => configurations.SelectMany(c => c.Pairs).Distinct();

    public override IEnumerable<ConfigurationPath> Paths => configurations.SelectMany(c => c.Paths).Distinct();

    public override IEnumerable<ConfigurationValue> Values => configurations.SelectMany(c => c.Values).Distinct();

    public override bool HasSection(ConfigurationPath prefix) => configurations.Any(c => c.HasSection(prefix));

    public override bool HasValue(ConfigurationPath fullPath) => configurations.Any(c => c.HasValue(fullPath));

    public override ConfigurationValue GetValue(ConfigurationPath fullPath) => configurations.FirstOrDefault(c => c.HasValue(fullPath)).GetValue(fullPath);

    public override void SetValue(ConfigurationPath fullPath, ConfigurationValue value) => configurations.FirstOrDefault(c => c.HasValue(fullPath))?.SetValue(fullPath, value);

    public override IConfiguration Clone() => new MultiConfiguration(Name, Options, configurations);
}
