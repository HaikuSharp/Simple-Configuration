using System.Collections.Generic;

namespace SC.Abstraction;

/// <summary>
/// Represents a configuration option that contains a list of items of specified type.
/// Combines functionality of a configuration option with a list collection.
/// </summary>
/// <typeparam name="TItem">The type of items in the list.</typeparam>
public interface IListConfigurationOption<TItem> : IConfigurationOption, IList<TItem>;
