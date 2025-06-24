namespace SC.Abstraction;

public interface IConfigurationSource
{
    IConfiguration Create(IConfigurationOptions options);
}
