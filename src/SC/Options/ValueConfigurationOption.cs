using SC.Abstraction;

namespace SC.Options;

/// <summary>
/// Represents a typed value configuration option.
/// </summary>
public sealed class ValueConfigurationOption<TValue> : IValueConfigurationOption<TValue>
{
    /// <summary>
    /// Gets or sets the value of the configuration option.
    /// </summary>
    public TValue Value { get; set; }

    /// <inheritdoc/>
    public void Save(string path, IConfigurationValueSource valueSource) => valueSource.SetRaw(path, Value);

    /// <inheritdoc/>
    public void Load(string path, IConfigurationValueSource valueSource)
    {
        if(!valueSource.TryGetRaw<TValue>(path, out var value)) return;
        Value = value;
    }
}
