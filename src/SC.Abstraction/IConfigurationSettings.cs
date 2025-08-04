namespace SC.Abstraction;

public interface IConfigurationSettings
{
    string Separator { get; }

    string SectionNameFormat { get; }

    int InitializeCapacity { get; }
}
