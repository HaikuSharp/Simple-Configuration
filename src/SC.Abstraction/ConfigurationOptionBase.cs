namespace SC.Abstraction;

/// <summary>
/// Base option class.
/// </summary>
public abstract class ConfigurationOptionBase : IConfigurationOption
{
    /// <inheritdoc/>
    public string Path { get; private set; }

    /// <inheritdoc/>
    public string Name { get; private set; }

    /// <inheritdoc/>
    public void Load(IConfigurationValueSource valueSource) => Load(Path, valueSource);

    /// <inheritdoc/>
    public void Save(IConfigurationValueSource valueSource) => Save(Path, valueSource);

    /// <summary>
    /// Create new option with path and name.
    /// </summary>
    /// <typeparam name="TOption">Option type.</typeparam>
    /// <param name="path">The option path.</param>
    /// <param name="name">The option name</param>
    /// <returns></returns>
    public static TOption Create<TOption>(string path, string name) where TOption : ConfigurationOptionBase, new() => new()
    {
        Path = path,
        Name = name
    };

    /// <summary>
    /// Saves the option value to the specified value source.
    /// </summary>
    /// <param name="path">The source path.</param>
    /// <param name="valueSource">The value source to save to.</param>
    protected abstract void Load(string path, IConfigurationValueSource valueSource);

    /// <summary>
    /// Loads the option value from the specified value source.
    /// </summary>
    /// <param name="path">The source path.</param>
    /// <param name="valueSource">The value source to load from.</param>
    protected abstract void Save(string path, IConfigurationValueSource valueSource);
}
