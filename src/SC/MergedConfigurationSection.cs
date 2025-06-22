using SC.Abstraction;

namespace SC;

public class MergedConfigurationSection(IConfigurationSection root, IConfigurationSection reference) : ConfigurationSectionBase(reference.Name)
{
    private readonly IConfigurationSection m_Section = reference.Clone();

    public override T GetValue<T>(ConfigurationPathEnumerator enumerator) => m_Section.TryGetValue(enumerator, out T value) ? value : root.GetValue<T>(enumerator);

    public override void SetValue<T>(ConfigurationPathEnumerator enumerator, T value) => m_Section.SetValue(enumerator, value);

    public override bool TryGetValue<T>(ConfigurationPathEnumerator enumerator, out T value) => m_Section.TryGetValue(enumerator, out value) || root.TryGetValue(enumerator, out value);

    public override bool TrySetValue<T>(ConfigurationPathEnumerator enumerator, T value) => m_Section.TrySetValue(enumerator, value);

    public override T ToObject<T>() => m_Section.ToObject<T>();

    public override IConfigurationSection Clone() => new MergedConfigurationSection(root, m_Section);
}