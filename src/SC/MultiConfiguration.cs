using SC.Abstraction;
using System.Collections.Generic;
using System.Linq;

namespace SC;

public class MultiConfiguration(string name, IConfigurationOptions options, IEnumerable<IConfiguration> configurations) : ConfigurationBase(name, options)
{
    public override bool HasSection(string prefix) => configurations.Any(c => c.HasSection(prefix));

    public override bool HasValue(string fullPath) => configurations.Any(c => c.HasValue(fullPath));

    public override string GetValue(string fullPath) => configurations.FirstOrDefault(c => c.HasValue(fullPath))?.GetValue(fullPath);

    public override void SetValue(string fullPath, string value) => configurations.FirstOrDefault(c => c.HasValue(fullPath))?.SetValue(fullPath, value);

    public override IConfiguration Clone() => new MultiConfiguration(Name, Options, configurations);
}
