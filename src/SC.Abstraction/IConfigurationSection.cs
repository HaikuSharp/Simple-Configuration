namespace SC.Abstraction;

public interface IConfigurationSection : IConfiguration, IHasConfigurationPath
{
    string GetAbsolutePath(string path);
}
