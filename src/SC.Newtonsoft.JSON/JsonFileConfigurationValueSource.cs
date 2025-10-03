using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SC.Abstraction;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SC.Newtonsoft.JSON;

/// <summary>
/// Represents a configuration values source that loads from and saves to Json file.
/// </summary>
public class JsonFileConfigurationValueSource(string filePath, IConfigurationSettings settings) : IFileConfigurationValueSource
{
    private readonly IConfigurationSettings m_Settings = settings;
    private JsonConfigurationValueSource m_Source = new(new JObject(), settings);

    /// <inheritdoc/>
    public string FilePath => filePath;

    /// <inheritdoc/>
    public bool HasRaw(string path) => m_Source.HasRaw(path);

    /// <inheritdoc/>
    public IEnumerable<string> GetRawsNames(string path) => m_Source.GetRawsNames(path);

    /// <inheritdoc/>
    public bool TryGetRaw<T>(string path, out T rawValue) => m_Source.TryGetRaw(path, out rawValue);

    /// <inheritdoc/>
    public T GetRaw<T>(string path) => m_Source.GetRaw<T>(path);

    /// <inheritdoc/>
    public void SetRaw<T>(string path, T rawValue) => m_Source.SetRaw(path, rawValue);

    /// <inheritdoc/>
    public void RemoveRaw(string path) => m_Source.RemoveRaw(path);

    /// <inheritdoc/>
    public void Clear() => m_Source.Clear();

    /// <inheritdoc/>
    public void Load()
    {
        if(!File.Exists(filePath)) throw new FileNotFoundException(filePath);

        m_Source.Load();

        using StreamReader streamReader = new(filePath);
        using JsonTextReader jsonReader = new(streamReader);

        m_Source = new(JToken.Load(jsonReader), m_Settings);
    }

    /// <inheritdoc/>
    public void Save()
    {
        var token = m_Source.NotNullSource;

        string directory = Path.GetDirectoryName(filePath);

        if(!string.IsNullOrEmpty(directory) && !Directory.Exists(directory)) _ = Directory.CreateDirectory(directory);

        using StreamWriter streamWriter = new(filePath);
        using JsonTextWriter jsonWriter = new(streamWriter)
        {
            Formatting = Formatting.Indented
        };

        token.WriteTo(jsonWriter);
    }

    /// <inheritdoc/>
    public async Task LoadAsync()
    {
        if(!File.Exists(filePath)) throw new FileNotFoundException(filePath);

        using FileStream fileStream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);

        using StreamReader streamReader = new(fileStream);
        using JsonTextReader jsonReader = new(streamReader);

        m_Source = new(await JToken.LoadAsync(jsonReader).ConfigureAwait(false), m_Settings);
    }

    /// <inheritdoc/>
    public async Task SaveAsync()
    {
        var source = m_Source.NotNullSource;

        string directory = Path.GetDirectoryName(filePath);

        if(!string.IsNullOrEmpty(directory) && !Directory.Exists(directory)) _ = Directory.CreateDirectory(directory);

        using FileStream fileStream = new(filePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);

        using StreamWriter streamWriter = new(fileStream);
        using JsonTextWriter jsonWriter = new(streamWriter)
        {
            Formatting = Formatting.Indented
        };

        await source.WriteToAsync(jsonWriter).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public void RemoveExcept(params IEnumerable<string> paths) => m_Source.RemoveExcept(paths);
}