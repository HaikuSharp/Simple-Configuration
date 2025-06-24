namespace SC.Abstraction;

public interface IConfigurationOptions
{
    string Separator { get; }
}

public class DefaultConfigurationOptions : IConfigurationOptions
{
    public static DefaultConfigurationOptions Default => field ??= new DefaultConfigurationOptions();

    public string Separator => ":";
}