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

            var a = 1;

            var deserializedSlackMessage = JsonConvert.DeserializeObject<SlackMessage>(Encoding.UTF8.GetString(response));
            
            //todo: assert that all the data exists correct in the output

        }
    }
}