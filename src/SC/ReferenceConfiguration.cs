using SC.Abstraction;

namespace SC;

public class ReferenceConfiguration(IConfiguration primary, IConfiguration reference) : ConfigurationBase(reference.Name, reference.Options)
{
    private readonly IConfiguration m_Reference = reference.Clone();

    public override bool HasSection(string prefix) => m_Reference.HasSection(prefix) || primary.HasSection(prefix);

    public override bool HasValue(string fullPath) => m_Reference.HasValue(fullPath) || primary.HasValue(fullPath);

    public override string GetValue(string fullPath) => m_Reference.GetValue(fullPath) ?? primary.GetValue(fullPath);

    public override void SetValue(string fullPath, string value) => m_Reference.SetValue(fullPath, value);

    public override IConfiguration Clone() => new ReferenceConfiguration(primary, reference);
}
