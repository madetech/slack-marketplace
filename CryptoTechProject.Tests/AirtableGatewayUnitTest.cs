using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using NUnit.Framework;
using FluentSim;

namespace CryptoTechProject.Tests
{
    [TestFixture]
    
    public class AirtableGatewayUnitTest
    {
        private string AIRTABLE_API_KEY = "111";
        private string TABLE_ID = "apppGaPfIFW8nmf4T";
        public FluentSimulator simulator = new FluentSimulator("http://localhost:8080/");
        [Test]
        public void CanGetAResponse()
        {
            //string path = "v0/apppGaPfIFW8nmf4T/Marketplace?api_key=" + apiKey + "&maxRecords=1";
            simulator.Start();
            Console.WriteLine(AIRTABLE_API_KEY);
            //AirtableGateway gateway = new AirtableGateway();
            simulator.Get("/v0/"+TABLE_ID+"/Marketplace" ).WithParameter("maxRecords", "1").WithParameter("api_key", AIRTABLE_API_KEY).Responds("Coding Black Females");
            
            var webClient = new WebClient();
            webClient.QueryString.Add("api_key", AIRTABLE_API_KEY);

            webClient.QueryString.Add("maxRecords", "1");

            var response = webClient.DownloadString("http://localhost:8080/v0/"+TABLE_ID+"/Marketplace"  );
            Assert.True(response.Contains("Coding Black Females"));
            simulator.Stop();


        }
     

    }
    
    
}