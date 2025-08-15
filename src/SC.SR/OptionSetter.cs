using SC.Abstraction;
using SR.Abstraction;

namespace SC.SR;

/// <summary>
/// Represents a setter for configuration option values.
/// </summary>
/// <typeparam name="T">The type of the option value.</typeparam>
public class OptionSetter<T>(IConfigurationOption<T> option) : ISetter<T>
{
    /// <inheritdoc/>
    public bool Set(T value)
    {
        option.Value = value;
        return true;
    }
}