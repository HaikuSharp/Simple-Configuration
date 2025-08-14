namespace SC.Abstraction;

public interface IReadOnlyConfigurationSection : IReadOnlyConfiguration, IHasConfigurationPath
{
    string GetAbsolutePath(string path);
}
