using SC.Abstraction;
using System.Collections.Generic;

namespace SC.Memory;

/// <summary>
/// Represents a configuration source that stores values in memory.
/// </summary>
public class MemoryConfigurationSource(string name, IDictionary<string, object> source) : ConfigurationSourceBase(name)
{
    /// <inheritdoc/>
    protected override IConfigurationValueSource GetValueSource(IConfigurationSettings settings) => new MemoryConfigurationValueSource(source);
}