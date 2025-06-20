namespace SC.Abstraction;

public interface IConfigurationBuilder
{
    IConfigurationSection Build();

    IConfigurationBuilder AppendSource(IConfigurationSource source);
}