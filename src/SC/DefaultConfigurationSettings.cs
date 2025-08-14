using SC.Abstraction;
using SC.Extensions;

namespace SC;

public sealed class DefaultConfigurationSettings : IConfigurationSettings
{
    public static DefaultConfigurationSettings Default => field ??= new();

    public string Separator => ":";

    public string SectionNameFormat => this.CombinePaths("{0}", "{1}");

    public int InitializeCapacity => 32;
}
