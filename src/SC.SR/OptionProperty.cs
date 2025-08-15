using SC.Abstraction;
using SR.Abstraction;

namespace SC.SR;

/// <summary>
/// Represents a property for configuration option values.
/// </summary>
/// <typeparam name="T">The type of the option value.</typeparam>
public class OptionProperty<T>(IConfigurationOption<T> option) : IProperty<T>
{
    /// <inheritdoc/>
    public T Get() => option.Value;

    /// <inheritdoc/>
    public bool Set(T value)
    {
        option.Value = value;
        return true;
    }
}