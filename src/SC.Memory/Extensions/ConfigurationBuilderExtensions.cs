using SC.Abstraction;
using System.Collections.Generic;

namespace SC.Memory.Extensions;

public static class ConfigurationBuilderExtensions
{
    public static IConfigurationBuilder AppendMemory(this IConfigurationBuilder builder, string name, IDictionary<string, object> source) => builder.Append(new MemoryConfigurationSource(name, source));
}
