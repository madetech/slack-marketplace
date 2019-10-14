using System;
using System.Net;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CryptoTechProject.Tests
{
    [TestFixture]
    public class DeliveryMechanismTest
    {
        [Test]
        public void CanRespondWithJson()
        {
            var thread = new Thread(() =>
            {
                Environment.SetEnvironmentVariable("PORT", "5001");
                //todo: consider passing a use case test double here 
                var deliveryMechanism = new DeliveryMechanism();
                deliveryMechanism.Run();
            });
            thread.Start();
            Thread.Sleep(5000);

            var webClient = new WebClient();
            var response = webClient.DownloadData("http://localhost:5001");

           
            dynamic deserializedSlackMessage = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(response));

            string secondWorkshop =  deserializedSlackMessage.blocks[3].text.text.ToString();
            
            Assert.True(secondWorkshop.Contains("Account Leadership - Roles & Responsibilities"));
            Assert.True(secondWorkshop.Contains("18/10/2019 03:30 PM"));
            Assert.True(secondWorkshop.Contains("Rory"));
        }
    }
}