using System;
using System.Collections.Generic;
using System.Net;
using CryptoTechProject.Domain;

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
            //WebClient webClient = new WebClient();

            DateTime sourceDate = new DateTime(2019, 10, 18, 14, 00, 0);
            DateTimeOffset time = new DateTimeOffset(sourceDate, 
                TimeZoneInfo.FindSystemTimeZoneById("Europe/London").GetUtcOffset(sourceDate));
            
            //webClient.QueryString.Add("api_key", _apiKey);

            //var response = webClient.DownloadString(_url+_tableId+"/Marketplace"  );
            
            return new List<Workshop>()
            {
                new Workshop()
                {
                    name = "Team Performance: Team Agile-Lean maturity 'measures' in practice (at DfE and Hackney)",
                    host = "Barry",
                    time = time,
                    location = "Everest, 2nd Foor",
                    duration = 60,
                    type = "Seminar"
                },
            };
        }
    }
}