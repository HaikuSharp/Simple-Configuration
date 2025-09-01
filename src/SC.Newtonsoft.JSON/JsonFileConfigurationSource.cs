using SC.Abstraction;
using System.IO;

namespace SC.Newtonsoft.JSON;

/// <summary>
/// Represents a configuration source that loads from and saves to JSON files.
/// </summary>
public class JsonFileConfigurationSource(string filePath) : ConfigurationSourceBase(Path.GetFileNameWithoutExtension(filePath))
{
    /// <inheritdoc/>
    protected override IConfigurationValueSource GetValueSource(IConfigurationSettings settings) => new JsonFileConfigurationValueSource(filePath, settings);
}
