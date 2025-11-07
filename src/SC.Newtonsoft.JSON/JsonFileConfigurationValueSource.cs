using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SC.Abstraction;
using System.IO;
using System.Threading.Tasks;

namespace SC.Newtonsoft.JSON;

/// <summary>
/// Represents a configuration values source that loads from and saves to Json file.
/// </summary>
public class JsonFileConfigurationValueSource(string filePath, IConfigurationSettings settings) : JsonConfigurationValueSource(new JObject(), settings)
{
    private readonly IConfigurationSettings m_Settings = settings;

    /// <inheritdoc/>
    public string FilePath => filePath;

    /// <inheritdoc/>
    public override void Load()
    {
        if(!File.Exists(filePath)) throw new FileNotFoundException(filePath);

        using StreamReader streamReader = new(filePath);
        using JsonTextReader jsonReader = new(streamReader);

        NotNullSource = JToken.Load(jsonReader);
    }

    /// <inheritdoc/>
    public override void Save()
    {
        var token = NotNullSource;

        string directory = Path.GetDirectoryName(filePath);

        if(!string.IsNullOrEmpty(directory) && !Directory.Exists(directory)) _ = Directory.CreateDirectory(directory);

        using StreamWriter streamWriter = new(filePath);
        using JsonTextWriter jsonWriter = new(streamWriter)
        {
            Formatting = Formatting.Indented
        };

        token.WriteTo(jsonWriter, []);
    }

    /// <inheritdoc/>
    public override async Task LoadAsync()
    {
        if(!File.Exists(filePath)) throw new FileNotFoundException(filePath);

        using FileStream fileStream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);

        using StreamReader streamReader = new(fileStream);
        using JsonTextReader jsonReader = new(streamReader);

        NotNullSource = await JToken.LoadAsync(jsonReader).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async Task SaveAsync()
    {
        var source = NotNullSource;

        string directory = Path.GetDirectoryName(filePath);

        if(!string.IsNullOrEmpty(directory) && !Directory.Exists(directory)) _ = Directory.CreateDirectory(directory);

        using FileStream fileStream = new(filePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);

        using StreamWriter streamWriter = new(fileStream);
        using JsonTextWriter jsonWriter = new(streamWriter)
        {
            Formatting = Formatting.Indented
        };

        await source.WriteToAsync(jsonWriter, []).ConfigureAwait(false);
    }
}