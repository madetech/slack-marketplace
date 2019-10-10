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

            string firstWorkshop =  deserializedSlackMessage.blocks[2].text.text.ToString();
            
            Assert.True(firstWorkshop.Contains("Coding Black Females - Code Dojo"));
            Assert.True(firstWorkshop.Contains("01/05/2008 08:30 AM"));
            Assert.True(firstWorkshop.Contains("Made Tech"));
        }
    }
}