using Newtonsoft.Json;

namespace CryptoTechProject
{
    public class ResponseThing
    {
        [JsonProperty("text")] public string Text;
        [JsonProperty("response_type")] public string ResponseType = "ephemeral";
    }
    
}