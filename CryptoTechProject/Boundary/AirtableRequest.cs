using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;

namespace CryptoTechProject.Boundary
{
    public class AirtableRequest
    {
        [JsonProperty("records")] public Record[] Records;
        [JsonProperty("typecast")] public bool Typecast = true;
    }

    public class Record
    {
        [JsonProperty("id")] public string ID;
        [JsonProperty("fields")] public Fields Fields;
    }

    public class Fields
    {
        public List<string> Attendees = new List<string>();
    }
}