namespace SC.Abstraction;

public interface IConfigurationBuilder
{
    IConfiguration Build(string name, IConfigurationOptions options);

    IConfigurationBuilder AppendSource(IConfigurationSource source);
}
