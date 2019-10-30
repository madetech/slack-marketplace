using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using CryptoTechProject.Boundary;
using NUnit.Framework;

namespace CryptoTechProject.Tests
{
    [TestFixture]
    public class DeliveryMechanismTest
    {
        [Test]
        public void CanGetNoWorkshopsAsSlackMessage()
        {
            var started = false;
            var deliveryMechanism = new DeliveryMechanism(null, new AlwaysNoWorkshops(), "5051");
            var thread = new Thread(() =>
            {
                deliveryMechanism.Run(() => { started = true; });
            });
            thread.Start();
            SpinWait.SpinUntil(() => started);
            var webClient = new WebClient();
            var responseBody = webClient.DownloadString("http://localhost:5051/");

            Assert.AreEqual(
                "{\"blocks\":[{\"type\":\"section\",\"text\":{\"type\":\"mrkdwn\",\"text\":\"*Workshops*\"}},{\"type\":\"divider\"}]}",
                responseBody
                );
            
        }
        
        [Test]
        public void CanGetThreeWorkshopsAsSlackMessage()
        {
            var started = false;
            var deliveryMechanism = new DeliveryMechanism(null, new AlwaysThreeWorkshops(), "5052");
            var thread = new Thread(() =>
            {
                deliveryMechanism.Run(() => { started = true; });
            });
            thread.Start();
            SpinWait.SpinUntil(() => started);
            var webClient = new WebClient();
            var responseBody = webClient.DownloadString("http://localhost:5052/");

            Assert.AreEqual(
                "{\"blocks\":[{\"type\":\"section\",\"text\":{\"type\":\"mrkdwn\",\"text\":\"*Workshops*\"}},{\"type\":\"divider\"},{\"type\":\"section\",\"text\":{\"type\":\"mrkdwn\",\"text\":\"**\\n01/01/2017 01:01 AM\\n\\nCurrent number of attendees: 0\"},\"accessory\":{\"type\":\"button\",\"text\":{\"type\":\"plain_text\",\"text\":\"Attend\"},\"value\":null}},{\"type\":\"section\",\"text\":{\"type\":\"mrkdwn\",\"text\":\"**\\n01/01/2019 01:01 AM\\n\\nCurrent number of attendees: 0\"},\"accessory\":{\"type\":\"button\",\"text\":{\"type\":\"plain_text\",\"text\":\"Attend\"},\"value\":null}},{\"type\":\"section\",\"text\":{\"type\":\"mrkdwn\",\"text\":\"**\\n01/01/2015 01:01 AM\\n\\nCurrent number of attendees: 0\"},\"accessory\":{\"type\":\"button\",\"text\":{\"type\":\"plain_text\",\"text\":\"Attend\"},\"value\":null}}]}",
                responseBody
            );
        }
        
        [Test]
        public void CanBookAttendance()
        {
            var started = false;
            var deliveryMechanism = new DeliveryMechanism(null, null, "5052");
            var thread = new Thread(() =>
            {
                deliveryMechanism.Run(() => { started = true; });
            });
            thread.Start();
            SpinWait.SpinUntil(() => started);
            
            

            
        }
    }

    public class AlwaysThreeWorkshops : IGetWorkshops
    {
        public GetWorkshopsResponse Execute()
        {
            return new GetWorkshopsResponse
            {
                PresentableWorkshops = new []
                {
                    new PresentableWorkshop
                    {
                        Time = new DateTimeOffset(2017, 1, 1, 1, 1, 1, TimeSpan.Zero),
                        Attendees = new List<string>()
                    },
                    new PresentableWorkshop
                    {
                        Time = new DateTimeOffset(2019, 1, 1, 1, 1, 1, TimeSpan.Zero),
                        Attendees = new List<string>()
                    },
                    new PresentableWorkshop
                    {
                        Time = new DateTimeOffset(2015, 1, 1, 1, 1, 1, TimeSpan.Zero),
                        Attendees = new List<string>()
                    }
                }
            };
        }
    }
    
    public class AlwaysNoWorkshops : IGetWorkshops
    {
        public GetWorkshopsResponse Execute()
        {
            return new GetWorkshopsResponse
            {
                PresentableWorkshops = new PresentableWorkshop[]{}
            };
        }
    }
}