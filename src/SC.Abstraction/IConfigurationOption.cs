using System;

namespace SC.Abstraction;

public interface IConfigurationOption<T> : IConfigurationOption
{
    new T Value { get; set; }
}

public interface IConfigurationOption
{
    string Path { get; }

    Type ValueType { get; }

    object Value { get; }

    void Save(IConfigurationValueSource valueSource);

    void Load(IConfigurationValueSource valueSource);
}