namespace SC.Abstraction;

public interface IConfigurationSource
{
    IConfigurationSection CreateSection();
}
