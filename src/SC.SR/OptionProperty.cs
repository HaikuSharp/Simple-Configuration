using SC.Abstraction;
using SR.Abstraction;

namespace SC.SR;

public class OptionProperty<T>(IConfigurationOption<T> option) : IProperty<T>
{
    public T Get() => option.Value;

    public bool Set(T value)
    {
        option.Value = value;
        return true;
    }
}