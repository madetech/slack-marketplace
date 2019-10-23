using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using CryptoTechProject.Boundary;
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
           
            webClient.QueryString.Add("maxRecords", "20");
            webClient.QueryString.Add("api_key", _apiKey);
            webClient.QueryString.Add("view", "Upcoming");

            var response = webClient.DownloadString(_url + "v0/"+_tableId + "/Marketplace");
            AirtableResponse airtableResponse = JsonConvert.DeserializeObject<AirtableResponse>(response);

            List<Workshop> allWorkshops = new List<Workshop>();

            for (int i = 0; i < airtableResponse.Records.Length; i++)
            {
                Workshop workshop = new Workshop()
                {
                    id = airtableResponse.Records[i].ID,
                    name = airtableResponse.Records[i].Fields.Name,
                    host = airtableResponse.Records[i].Fields.Host,
                    time = airtableResponse.Records[i].Fields.Time,
                    location = airtableResponse.Records[i].Fields.Location,
                    duration = airtableResponse.Records[i].Fields.Duration/60,
                    type = airtableResponse.Records[i].Fields.SessionType,
                    attendees = airtableResponse.Records[i].Fields.Attendees
                };
                allWorkshops.Add(workshop);
            }

            return allWorkshops;
        }

        public void Save(Workshop workshop)
        {
            WebRequest airtableRequest = WebRequest.Create("http://localhost:8080/");
            airtableRequest.Method = "PATCH";
            airtableRequest.Headers.Add("Authorization","Bearer API_KEY_GOES_HERE" );
            airtableRequest.ContentType = "application/json";
            AirtableRequest patchData = new AirtableRequest
            {
                ID = "recxAicGB8fU6PT0l",
                Fields = new Fields()
                {
                    Attendees = "Seaweed"
                }

            };
            string jsonPatchData = JsonConvert.SerializeObject(patchData);
            byte[] bytes = Encoding.UTF8.GetBytes(jsonPatchData);
            airtableRequest.ContentLength = bytes.Length;
            Stream dataStream = airtableRequest.GetRequestStream();
            dataStream.Write(bytes, 0, bytes.Length);
            dataStream.Close();

        }
    }
}