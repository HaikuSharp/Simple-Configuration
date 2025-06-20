using Newtonsoft.Json.Linq;
using SC.Abstraction;
using System.IO;

namespace SC.Newtonsoft.JSON;

public class JsonFileConfigurationSource(string fileName) : IConfigurationSource
{
    public IConfigurationSection CreateSection() => new JsonFileConfigurationSection(this, Path.GetFileNameWithoutExtension(fileName), LoadSection());

    public JToken LoadSection() => File.Exists(fileName) ? JToken.Parse(File.ReadAllText(fileName)) : throw new FileNotFoundException(null, fileName);

    public void SaveSection(JsonConfigurationSection section)
    {
        string directoryPath = Path.GetDirectoryName(fileName);
        if(!Directory.Exists(directoryPath)) throw new DirectoryNotFoundException(directoryPath);
        File.WriteAllText(fileName, section.Token.ToString());
    }
}
