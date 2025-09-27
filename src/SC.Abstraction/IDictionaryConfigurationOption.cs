using System.Collections.Generic;

namespace SC.Abstraction;

/// <summary>
/// Represents a configuration option that contains a dictionary with specified key and item types.
/// Combines functionality of a configuration option with a dictionary collection.
/// </summary>
/// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
/// <typeparam name="TItem">The type of values in the dictionary.</typeparam>
public interface IDictionaryConfigurationOption<TKey, TItem> : IConfigurationOption, IDictionary<TKey, TItem>;