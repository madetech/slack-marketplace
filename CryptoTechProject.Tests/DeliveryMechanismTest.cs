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
        [Ignore("Trying different gateway")]
        [Test]
        public void CanRespondWithJson()
        {
            var started = false;
            var thread = new Thread(() =>
            {
                Environment.SetEnvironmentVariable("PORT", "5001");
                //todo: consider passing a use case test double here 
                var deliveryMechanism = new DeliveryMechanism();
                deliveryMechanism.Run(() => { started = true; });
            });
            thread.Start();

            SpinWait.SpinUntil(() => started);

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