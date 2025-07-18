using SC.Abstraction;
using System.Collections.Generic;

namespace SC.Extensions;

public static class ConfigurationExtansions
{
    public static void Add(this IConfiguration configuration, ConfigurationPathValuePair pair) => configuration.Add(pair.Path, pair.Value);

    public static void Include(this IConfiguration configuration, IConfigurationSource source, IConfigurationOptions options) => configuration.Include(source.Create(options));

    public static void Include(this IConfiguration configuration, IConfiguration includeConfiguration) => configuration.Include(includeConfiguration.Pairs);

    public static void Include(this IConfiguration configuration, IEnumerable<ConfigurationPathValuePair> pairs)
    {
        foreach(var pair in pairs)
        {
            if(configuration.HasValue(pair.Path)) continue;
            configuration.Add(pair);
        }
    }
}
