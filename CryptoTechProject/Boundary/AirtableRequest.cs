using System.Net.Http;
using Newtonsoft.Json;

namespace CryptoTechProject.Boundary
{
    public class AirtableRequest
    {
        [JsonProperty("records")] public Record[] Records;
    }

    public class Record
    {
        [JsonProperty("id")] public string ID;
        [JsonProperty("fields")] public Fields Fields;
    }
    public class Fields
    {
        public string Attendees;
    }
}