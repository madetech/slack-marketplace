using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using CryptoTechProject.Boundary;
using CryptoTechProject.Domain;
using Newtonsoft.Json;


namespace CryptoTechProject
{
    public class AirtableGateway : IWorkshopsGateway, IFindWorkshopGateway
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

            var response = webClient.DownloadString(_url + "v0/" + _tableId + "/Marketplace");
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
                    duration = airtableResponse.Records[i].Fields.Duration / 60,
                    type = airtableResponse.Records[i].Fields.SessionType,
                    attendees = airtableResponse.Records[i].Fields.Attendees
                };
                allWorkshops.Add(workshop);
            }

            return allWorkshops;
        }

        public void Update(Workshop workshop)
        {
            AirtableRequest patchData = new AirtableRequest
            {
                Records = new Record[]
                {
                    new Record()
                    {
                        ID = workshop.id,
                        Fields = new Fields()
                        {
                            Attendees = workshop.attendees
                        }
                    }
                }
            };
            string jsonPatchData = JsonConvert.SerializeObject(patchData);
            Console.WriteLine(jsonPatchData);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            client.Headers.Add("Authorization", "Bearer " + _apiKey);
            client.Headers.Add("Content-Type", "application/json");
            string responseConfirmation = client.UploadString(_url + "v0/" + _tableId + "/Marketplace", "PATCH", jsonPatchData);
           //  string responseConfirmation = client.UploadString ("http://localhost:8080/", "PATCH", jsonPatchData);
        }

        public Workshop Find(string workshopID)
        {
            WebClient webClient = new WebClient();
            
            webClient.Headers.Add("Authorization", "Bearer " + _apiKey);
            webClient.Headers.Add("Content-Type", "application/json");


            var response = webClient.DownloadString(_url + "v0/" + _tableId + "/Marketplace/" + workshopID);
            AirtableResponse.WorkshopRecord airtableResponse = JsonConvert.DeserializeObject<AirtableResponse.WorkshopRecord>(response);

            Workshop workshop = new Workshop()
            {
                id = airtableResponse.ID,
                name = airtableResponse.Fields.Name,
                host = airtableResponse.Fields.Host,
                time = airtableResponse.Fields.Time,
                location = airtableResponse.Fields.Location,
                duration = airtableResponse.Fields.Duration / 60,
                type = airtableResponse.Fields.SessionType,
                attendees = airtableResponse.Fields.Attendees
            };
            return workshop;
        }
    }
}