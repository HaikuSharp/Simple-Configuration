namespace SC.Abstraction;

public interface IConfigurationOption<T> : IConfigurationOption
{
    new T Value { get; set; }
}

public interface IConfigurationOption
{
    string Path { get; }

    int Version { get; }

    object Value { get; }
}