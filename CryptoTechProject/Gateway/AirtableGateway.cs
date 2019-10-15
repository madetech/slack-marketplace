using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using CryptoTechProject.Domain;
using Newtonsoft.Json;

namespace CryptoTechProject
{
    public class AirtableGateway : IGetWorkshopsGateway
    {
        private string _url;
        private string _apiKey;
        private string _tableId;
        
        public AirtableGateway(string url, string apiKey, string tableId)
        {
            _url = url;
            _apiKey = apiKey;
            _tableId = tableId;

        }
        public List<Workshop> All()

        {
            WebClient webClient = new WebClient();
            webClient.QueryString.Add("api_key", _apiKey);
            var response = webClient.DownloadString(_url+"v0/"+_tableId+"/Marketplace"  );

            AirtableResponse airtableResponse = JsonConvert.DeserializeObject<AirtableResponse>(response);
            
            DateTime sourceDate = new DateTime(2019, 09, 18, 17, 00, 0);
            DateTimeOffset time = new DateTimeOffset(sourceDate, 
                TimeZoneInfo.FindSystemTimeZoneById("Europe/London").GetUtcOffset(sourceDate));
            //return deserializedProduct;
            return new List<Workshop>()
            {
                new Workshop()
                {
                    name = airtableResponse.Records[0].Fields.Name,
                    host = airtableResponse.Records[0].Fields.Host,
                    time = airtableResponse.Records[0].Fields.Time,
                    location = airtableResponse.Records[0].Fields.Location,
                    duration = airtableResponse.Records[0].Fields.Duration,
                    type = airtableResponse.Records[0].Fields.SessionType
                },
            };
        }
    }
}