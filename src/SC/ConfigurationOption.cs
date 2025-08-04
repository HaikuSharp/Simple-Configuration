using SC.Abstraction;
using System.Collections.Generic;

namespace SC;

public class ConfigurationOption<T>(string path, T value) : IConfigurationOption<T>
{
    public string Path => path;

    public int Version { get; private set; }

    public T Value
    { 
        get => field; 
        set 
        {
            if(EqualityComparer<T>.Default.Equals(field, value)) return;
            field = value;
            Version++;
        } 
    } = value;

    public static ConfigurationOption<T> CreateAsDirty(string path, T value)
    {
        ConfigurationOption<T> option = new(path, value);
        option.Version++;
        return option;
    }

    object IConfigurationOption.Value => Value;
}
