using System;
using System.Net;
using Newtonsoft.Json;
using System.Collections.Generic;

public class AirtableResponse
{
    [JsonProperty("records")] public WorkshopRecord[] Records;

    public class WorkshopRecord
    {
        [JsonProperty("id")] public string ID;
        [JsonProperty("fields")] public Fields Fields;
        [JsonProperty("createdTime")] public DateTime CreatedTime;
    }

    public class Fields
    {
        public string Host;
        public string Location;
        public string Name;
        public DateTimeOffset Time;
        public int Duration;
        [JsonProperty("Session Type")] public string SessionType;
        public string[] Categories;
    }
}