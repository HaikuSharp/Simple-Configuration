using SC.Abstraction;
using System.Collections.Generic;
using System.Linq;

namespace SC;

public sealed class ReferenceConfiguration(IConfiguration primary, IConfiguration reference) : ConfigurationBase(reference.Name, reference.Options)
{
    private readonly IConfiguration m_Reference = reference.Clone();

    public override IEnumerable<ConfigurationPathValuePair> Pairs => reference.Pairs.Concat(primary.Pairs).Distinct();

    public override IEnumerable<ConfigurationPath> Paths => reference.Paths.Concat(primary.Paths).Distinct();

    public override IEnumerable<ConfigurationValue> Values => reference.Values.Concat(primary.Values).Distinct();

    public override bool HasSection(ConfigurationPath prefix) => m_Reference.HasSection(prefix) || primary.HasSection(prefix);

    public override bool HasValue(ConfigurationPath fullPath) => m_Reference.HasValue(fullPath) || primary.HasValue(fullPath);

    public override ConfigurationValue GetValue(ConfigurationPath fullPath)
    {
        var refValue = m_Reference.GetValue(fullPath);
        return !refValue.IsEmpty ? refValue : primary.GetValue(fullPath);
    }

    public override void SetValue(ConfigurationPath fullPath, ConfigurationValue value) => m_Reference.SetValue(fullPath, value);

    public override IConfiguration Clone() => new ReferenceConfiguration(primary, reference);
}
