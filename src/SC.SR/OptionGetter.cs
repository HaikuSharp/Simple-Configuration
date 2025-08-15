using SC.Abstraction;
using SR.Abstraction;

namespace SC.SR;

/// <summary>
/// Represents a getter for configuration option values.
/// </summary>
/// <typeparam name="T">The type of the option value.</typeparam>
public class OptionGetter<T>(IReadOnlyConfigurationOption<T> option) : IGetter<T>
{
    /// <inheritdoc/>
    public T Get() => option.Value;
}