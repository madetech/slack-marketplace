using Newtonsoft.Json;

namespace CryptoTechProject
{
    public class SlackButtonPayload
    {
        [JsonProperty("user")] public User User;
        [JsonProperty("actions")] public Actions[] Actions;

        [JsonProperty("response_url")] public string ResponseURL;
    }

    public class User
    {
        [JsonProperty("name")] public string Name;
        [JsonProperty("id")] public string UserID;
    }

    public class Actions
    {
        [JsonProperty("value")] public string Value;
    }
}