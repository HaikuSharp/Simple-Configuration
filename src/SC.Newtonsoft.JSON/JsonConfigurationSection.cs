using Newtonsoft.Json.Linq;
using SC.Abstraction;

namespace SC.Newtonsoft.JSON;

public class JsonConfigurationSection(string name, JToken rootToken) : ConfigurationSectionBase(name)
{
    protected internal JToken Token { get; internal set; } = rootToken;

    public override T GetValue<T>(ConfigurationPathEnumerator enumerator) => GetToken(enumerator, Token).ToObject<T>();

    public override void SetValue<T>(ConfigurationPathEnumerator enumerator, T value) => GetToken(enumerator, Token).Replace(JToken.FromObject(value));

    public override T ToObject<T>() => Token.ToObject<T>();

    public override bool TryGetValue<T>(ConfigurationPathEnumerator enumerator, out T value)
    {
        if(TryGetToken(enumerator, Token, out var token))
        {
            value = token.ToObject<T>();
            return true;
        }

        value = default;
        return false;
    }

    public override bool TrySetValue<T>(ConfigurationPathEnumerator enumerator, T value)
    {
        if(!TryGetToken(enumerator, Token, out var token)) return false;
        token.Replace(JToken.FromObject(value));
        return true;
    }

    public override IConfigurationSection Clone() => new JsonConfigurationSection(Name, Token);

    private static JToken GetToken(ConfigurationPathEnumerator enumerator, JToken token)
    {
        while(enumerator.MoveNext()) token = token[enumerator.Current];
        return token;
    }

    private static bool TryGetToken(ConfigurationPathEnumerator enumerator, JToken root, out JToken token)
    {
        token = root;

        while(enumerator.MoveNext())
        {
            var ntoken = token[enumerator.Current];
            if(ntoken is null) return false;
            token = ntoken;
        }

        return token != null;
    }
}
