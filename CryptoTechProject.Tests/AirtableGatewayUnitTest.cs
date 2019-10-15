using System.Net;
using System.Text;
using NUnit.Framework;
using FluentSim;

namespace CryptoTechProject.Tests
{
    [TestFixture]
    
    public class AirtableGatewayUnitTest
    {
        public FluentSimulator simulator = new FluentSimulator("http://localhost:8080/");
        [Test]
        public void CanGetAResponse()
        {
          
            simulator.Start();

            AirtableGateway gateway = new AirtableGateway();
            simulator.Get("/airtable/API").Responds("Airtable response thing");
            
            var webClient = new WebClient();
            var response = webClient.DownloadData("http://localhost:8080/airtable/API");
            var strResponse = Encoding.UTF8.GetString(response);
            Assert.True(strResponse.Contains("Airtable"));
            simulator.Stop();


        }
     

    }
    
    
}