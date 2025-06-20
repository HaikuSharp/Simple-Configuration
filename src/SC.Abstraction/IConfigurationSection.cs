namespace SC.Abstraction;

public interface IConfigurationSection : IToObjectConvertable
{
    string Name { get; }

    T GetValue<T>(ConfigurationPath path);

    void SetValue<T>(ConfigurationPath path, T value);

    T GetValue<T>(ConfigurationPathEnumerator path);

    void SetValue<T>(ConfigurationPathEnumerator path, T value);
}
