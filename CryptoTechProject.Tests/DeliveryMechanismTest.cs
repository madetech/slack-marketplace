using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Web;
using CryptoTechProject.Boundary;
using NUnit.Framework;
using FluentSim;
using Newtonsoft.Json;


namespace CryptoTechProject.Tests
{
    [TestFixture]
    public class DeliveryMechanismTest
    
    {
        public FluentSimulator simulator;
        
        
        
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
            Assert.AreEqual("application/json", webClient.ResponseHeaders["Content-Type"]);
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
        
        [Ignore("Because")]
        [Test]
        public void CanBookAttendance()
        {
            var started = false;
            var spyToggleWorkshopAttendance = new SpyToggleWorkshopAttendance();
            var deliveryMechanism = new DeliveryMechanism(spyToggleWorkshopAttendance, null, "5053");
            var thread = new Thread(() =>
            {
                deliveryMechanism.Run(() => { started = true; });
            });
            thread.Start();
            SpinWait.SpinUntil(() => started);
            
            var webClient = new WebClient();
            webClient.UploadString("http://localhost:5053/attend","POST", "payload=%7B%22type%22%3A%22block_actions%22%2C%22team%22%3A%7B%22id%22%3A%22T0B0XJCTC%22%2C%22domain%22%3A%22madetechteam%22%7D%2C%22user%22%3A%7B%22id%22%3A%22UHN295AA0%22%2C%22username%22%3A%22tony%22%2C%22name%22%3A%22tony%22%2C%22team_id%22%3A%22T0B0XJCTC%22%7D%2C%22api_app_id%22%3A%22ANSD6N533%22%2C%22token%22%3A%22ceHc4m6dPr63SPqswlFQsfhD%22%2C%22container%22%3A%7B%22type%22%3A%22message%22%2C%22message_ts%22%3A%221572428564.002300%22%2C%22channel_id%22%3A%22CDCQLNMEF%22%2C%22is_ephemeral%22%3Atrue%7D%2C%22trigger_id%22%3A%22801820457346.11031624930.ef92ce73dd2d8e76340af4f241c28b1e%22%2C%22channel%22%3A%7B%22id%22%3A%22CDCQLNMEF%22%2C%22name%22%3A%22academy-2019-sum-aut%22%7D%2C%22response_url%22%3A%22https%3A%5C%2F%5C%2Fhooks.slack.com%5C%2Factions%5C%2FT0B0XJCTC%5C%2F814613123445%5C%2FYDP2bi3aPQWs2eFE3LKj58XC%22%2C%22actions%22%3A%5B%7B%22action_id%22%3A%22%3DtU%22%2C%22block_id%22%3A%223Yv%2Bu%22%2C%22text%22%3A%7B%22type%22%3A%22plain_text%22%2C%22text%22%3A%22Attend%22%2C%22emoji%22%3Atrue%7D%2C%22value%22%3A%22recWTcxEQVKjwpIIf%22%2C%22type%22%3A%22button%22%2C%22action_ts%22%3A%221572428585.731525%22%7D%5D%7D");

            Assert.AreEqual("tony", spyToggleWorkshopAttendance._called.User);
            Assert.AreEqual("recWTcxEQVKjwpIIf", spyToggleWorkshopAttendance._called.WorkshopId);


        }
        
        [Test]
        public void CanDeliverSlackMessageWithCorrectButton()
        {
            FluentSimulator slackSimulator = new FluentSimulator("http://localhost:8081/");
            string request_url_path =
                "/actions";
            slackSimulator.Post(request_url_path).Responds("");
            slackSimulator.Start();
            
            var started = false;
            var spyToggleWorkshopAttendance = new SpyToggleWorkshopAttendance();
            var spyGetWorkshop = new StubGetWorkshop();
            spyGetWorkshop.attendees.Add("Bing");
            var deliveryMechanism = new DeliveryMechanism(spyToggleWorkshopAttendance, spyGetWorkshop, "5054");
                        
             var thread = new Thread(() =>
            {
                deliveryMechanism.Run(() => { started = true; });
            });
            thread.Start();
            SpinWait.SpinUntil(() => started);

            SlackButtonPayload payload = new SlackButtonPayload()
            {
                User = new User() {Name = "Bing", UserID = "123"},
                Actions = new Actions[] {new Actions() {Value = "record3"}},
                ResponseURL = "http://localhost:8081/actions"
            };

            string firstjson = JsonConvert.SerializeObject(payload);
            var encoded = HttpUtility.UrlEncode(firstjson);
            
            
            var fakeSlackWebClient = new WebClient();
            fakeSlackWebClient.UploadString("http://localhost:5054/attend", "POST", "payload="+encoded);

            var requestReceivedBySlack = slackSimulator.ReceivedRequests;
            Assert.AreEqual("application/json",requestReceivedBySlack[0].ContentType);
            Assert.IsTrue(requestReceivedBySlack[0].RequestBody.Contains("Unattend"));
            
            
            slackSimulator.Stop();
        }
        
        [Test]
        public void ShowcaseDoesNotContainAttendButtonAndNumberOfAttendees()
        {
            var started = false;
            var deliveryMechanism = new DeliveryMechanism(null, new AlwaysOneShowcase(), "5049");
            var thread = new Thread(() =>
            {
                deliveryMechanism.Run(() => { started = true; });
            });
            thread.Start();
            SpinWait.SpinUntil(() => started);
            var webClient = new WebClient();
            var responseBody = webClient.DownloadString("http://localhost:5049/");

            Assert.IsFalse(responseBody.Contains("button"));
            Assert.IsFalse(responseBody.Contains("accessory"));
            Assert.IsFalse(responseBody.Contains("Current number of attendees"));
            Assert.AreEqual("application/json", webClient.ResponseHeaders["Content-Type"]);
        }
    }

    public class SpyToggleWorkshopAttendance : IToggleWorkshopAttendance
    {
        public ToggleWorkshopAttendanceRequest _called;

        public string Execute(ToggleWorkshopAttendanceRequest request)
        {
            _called = request;
            return "";
        }
    }
    
    public class StubGetWorkshop : IGetWorkshops
    {
        public List<string> attendees = new List<string>();
        public GetWorkshopsResponse Execute()
        {
            return new GetWorkshopsResponse()
            {
                PresentableWorkshops = new PresentableWorkshop[]
                {
                    new PresentableWorkshop()
                    {
                        Name = "A",
                        Time = new DateTimeOffset(2019, 1, 1, 1, 1, 1, TimeSpan.Zero),
                        Host = "B",
                        Attendees = attendees
                    }
                }
            };
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
    
    public class AlwaysOneShowcase : IGetWorkshops
    {
        public GetWorkshopsResponse Execute()
        {
            return new GetWorkshopsResponse
            {
                PresentableWorkshops = new []
                {
                    new PresentableWorkshop
                    {
                        Type = "Showcase",
                        Time = new DateTimeOffset(2019, 1, 1, 4, 30, 1, TimeSpan.Zero),
                        Attendees = new List<string>()
                    },
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