namespace SC.Abstraction;

public interface IConfigurationOption<T> : IConfigurationOption, IReadOnlyConfigurationOption<T>
{
    new T Value { get; set; }
}

public interface IConfigurationOption : IReadOnlyConfigurationOption
{
    new object Value { get; set; }

    void Save(IConfigurationValueSource valueSource);

    void Load(IConfigurationValueSource valueSource);
}
