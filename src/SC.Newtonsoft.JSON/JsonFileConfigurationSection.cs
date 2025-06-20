using Newtonsoft.Json.Linq;

namespace SC.Newtonsoft.JSON;

public class JsonFileConfigurationSection(JsonFileConfigurationSource source, string name, JToken rootToken) : JsonConfigurationSection(name, rootToken)
{
    public void Load() => Token = source.LoadSection();

    public void Save() => source.SaveSection(this);
}
