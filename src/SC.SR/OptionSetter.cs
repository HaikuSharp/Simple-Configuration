using SC.Abstraction;
using SR.Abstraction;

namespace SC.SR;

public class OptionSetter<T>(IConfigurationOption<T> option) : ISetter<T>
{
    public bool Set(T value)
    {
        option.Value = value;
        return true;
    }
}
