namespace SC.Abstraction;

public interface IConfigurationBuilder
{
    IConfigurationRoot Build(string name, IConfigurationSettings settings);

    IConfigurationBuilder Append(IConfigurationSource source);
}
