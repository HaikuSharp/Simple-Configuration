using SC.Abstraction;
using SR.Abstraction;

namespace SC.SR;

public class OptionGetter<T>(IConfigurationOption<T> option) : IGetter<T>
{
    public T Get() => option.Value;
}
