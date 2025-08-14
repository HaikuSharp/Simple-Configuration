using SC.Abstraction;
using SC.Extensions;

namespace SC.SR.Extensions;

public static class ConfigurationExtensions
{
    public static bool TryGetValueGetter<T>(this IReadOnlyConfiguration configuration, string path, out OptionGetter<T> getter) => (getter = configuration.GetValueGetter<T>(path)) is not null;

    public static bool TryGetValueSetter<T>(this IConfiguration configuration, string path, out OptionSetter<T> setter) => (setter = configuration.GetValueSetter<T>(path)) is not null;

    public static bool TryGetValueProperty<T>(this IConfiguration configuration, string path, out OptionProperty<T> property) => (property = configuration.GetValueProperty<T>(path)) is not null;

    public static OptionGetter<T> GetValueGetter<T>(this IReadOnlyConfiguration configuration, string path) => configuration.TryGetOption<T>(path, out var option) ? new(option) : null;

    public static OptionSetter<T> GetValueSetter<T>(this IConfiguration configuration, string path) => configuration.TryGetOption<T>(path, out var option) ? new(option) : null;

    public static OptionProperty<T> GetValueProperty<T>(this IConfiguration configuration, string path) => configuration.TryGetOption<T>(path, out var option) ? new(option) : null;
}
