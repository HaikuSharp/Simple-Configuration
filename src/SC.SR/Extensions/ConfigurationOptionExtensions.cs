using SC.Abstraction;

namespace SC.SR.Extensions;

public static class ConfigurationOptionExtensions
{
    public static OptionGetter<T> AsGetter<T>(IReadOnlyConfigurationOption<T> option) => new(option);

    public static OptionSetter<T> AsSetter<T>(IConfigurationOption<T> option) => new(option);

    public static OptionProperty<T> AsProperty<T>(IConfigurationOption<T> option) => new(option);
}
