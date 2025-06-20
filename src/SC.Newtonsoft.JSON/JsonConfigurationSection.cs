using Newtonsoft.Json.Linq;
using SC.Abstraction;

namespace SC.Newtonsoft.JSON;

public class JsonConfigurationSection(string name, JToken rootToken) : ConfigurationSectionBase(name)
{
    protected internal JToken Token { get; internal set; } = rootToken;

    public override T GetValue<T>(ConfigurationPathEnumerator enumerator) => GetToken(enumerator, Token).ToObject<T>();

    public override void SetValue<T>(ConfigurationPathEnumerator enumerator, T value) => GetToken(enumerator, Token).Replace(JToken.FromObject(value));

    public override T ToObject<T>() => Token.ToObject<T>();

    private static JToken GetToken(ConfigurationPathEnumerator enumerator, JToken token)
    {
        while(enumerator.MoveNext()) token = token[enumerator.Current];
        return token;
    }
}
