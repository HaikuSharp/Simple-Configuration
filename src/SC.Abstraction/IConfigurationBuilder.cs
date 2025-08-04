namespace SC.Abstraction;

public interface IConfigurationBuilder
{
    IConfiguration Build(string name, IConfigurationSettings settings);

    IConfigurationBuilder Append(IConfigurationSource source);
}
