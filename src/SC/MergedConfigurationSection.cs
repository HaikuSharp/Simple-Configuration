using SC.Abstraction;

namespace SC;

public class MergedConfigurationSection(IConfigurationSection primary, IConfigurationSection reference) : ConfigurationSectionBase(reference.Name)
{
    private readonly IConfigurationSection m_Reference = reference.Clone();

    public override T GetValue<T>(ConfigurationPathEnumerator enumerator) => primary.TryGetValue(enumerator, out T value) ? value : m_Reference.GetValue<T>(enumerator);

    public override void SetValue<T>(ConfigurationPathEnumerator enumerator, T value) => m_Reference.SetValue(enumerator, value);

    public override bool TryGetValue<T>(ConfigurationPathEnumerator enumerator, out T value) => primary.TryGetValue(enumerator, out value) || m_Reference.TryGetValue(enumerator, out value);

    public override bool TrySetValue<T>(ConfigurationPathEnumerator enumerator, T value) => m_Reference.TrySetValue(enumerator, value);

    public override T ToObject<T>() => m_Reference.ToObject<T>();

    public override IConfigurationSection Clone() => new MergedConfigurationSection(primary, m_Reference);
}