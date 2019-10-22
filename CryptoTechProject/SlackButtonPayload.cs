using Newtonsoft.Json;

namespace CryptoTechProject
{
    public class SlackButtonPayload
    {
        [JsonProperty("user")] public User User;
       // [JsonProperty("actions")] public Actions Actions;
    }

    public class User
    {
        [JsonProperty("name")] public string Name;
    }

    public class Actions
    {
        [JsonProperty("value")] public string Value;
    }
}