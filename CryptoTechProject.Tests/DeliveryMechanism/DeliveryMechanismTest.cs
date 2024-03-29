using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Web;
using CryptoTechProject.Boundary;
using FluentAssertions;
using NUnit.Framework;
using FluentSim;
using Newtonsoft.Json;


namespace CryptoTechProject.Tests
{
    [TestFixture]
    public class DeliveryMechanismTest
    
    {
        private static void StartServer(string port, IGetWorkshops stub)
        {
            var started = false;
            var deliveryMechanism = new DeliveryMechanism(null, stub, port);
            var thread = new Thread(() => { deliveryMechanism.Run(() => { started = true; }); });
            thread.Start();
            SpinWait.SpinUntil(() => started);
        }

        [Test]
        public void CanGetNoWorkshopsAsSlackMessage()
        {
            StartServer("5051", new AlwaysNoWorkshops());

            var webClient = new WebClient();
            var responseBody = webClient.DownloadString("http://localhost:5051/");

            var expectedJson = "{\"blocks\":[{\"type\":\"section\",\"text\":{\"type\":\"mrkdwn\",\"text\":\"*Sessions*\"}},{\"type\":\"divider\"}]}";
            
            responseBody.Should().Be(expectedJson);
            webClient.ResponseHeaders["Content-Type"].Should().Be("application/json");
        }

        [Test]
        public void CanGetThreeWorkshopsAsSlackMessage()
        {
            StartServer("5052", new AlwaysThreeWorkshops());

            var webClient = new WebClient();
            var responseBody = webClient.DownloadString("http://localhost:5052/");
            
           var expectedJson = "{\"blocks\":[{\"type\":\"section\",\"text\":{\"type\":\"mrkdwn\",\"text\":\"*Sessions*\"}},{\"type\":\"divider\"},{\"type\":\"section\",\"text\":{\"type\":\"mrkdwn\",\"text\":\"*01/01/2017*\"}},{\"type\":\"section\",\"text\":{\"type\":\"mrkdwn\",\"text\":\"**\\n01:01 - 01:02\\nRory in Everest\\nCurrent number of attendees: 0\"},\"accessory\":{\"type\":\"button\",\"text\":{\"type\":\"plain_text\",\"text\":\"Attend\"},\"value\":null}},{\"type\":\"divider\"},{\"type\":\"section\",\"text\":{\"type\":\"mrkdwn\",\"text\":\"*01/01/2019*\"}},{\"type\":\"section\",\"text\":{\"type\":\"mrkdwn\",\"text\":\"**\\n01:01 - 02:01\\nRory in Everest\\nCurrent number of attendees: 0\"},\"accessory\":{\"type\":\"button\",\"text\":{\"type\":\"plain_text\",\"text\":\"Attend\"},\"value\":null}},{\"type\":\"divider\"},{\"type\":\"section\",\"text\":{\"type\":\"mrkdwn\",\"text\":\"*01/01/2015*\"}},{\"type\":\"section\",\"text\":{\"type\":\"mrkdwn\",\"text\":\"**\\n01:01 - 03:01\\nRory in Everest\\nCurrent number of attendees: 0\"},\"accessory\":{\"type\":\"button\",\"text\":{\"type\":\"plain_text\",\"text\":\"Attend\"},\"value\":null}}]}";
            responseBody.Should().Be(expectedJson);
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
            requestReceivedBySlack[0].ContentType.Should().Be("application/json");
            requestReceivedBySlack[0].RequestBody.Should().Contain("Unattend");
            
            slackSimulator.Stop();
        }
        
        [Test]
        public void ShowcaseDoesNotContainAttendButtonAndNumberOfAttendees()
        {
            StartServer("5056", new AlwaysOneShowcase());

            var webClient = new WebClient();
            var responseBody = webClient.DownloadString("http://localhost:5056/");

            responseBody.Should().NotContain("button");
            responseBody.Should().NotContain("accessory");
            responseBody.Should().NotContain("Current number of attendees");
            webClient.ResponseHeaders["Content-Type"].Should().Be("application/json");
        }
        
        [Test]
        public void CheckHasDividerBetweenTwoSessionsWithDifferentDates()
        {
            StartServer("5049", new AlwaysTwoShowcasesAndOneWorkshop());

            var webClient = new WebClient();
            var responseBody = webClient.DownloadString("http://localhost:5049/");

            var expectedJson =
                "{\"blocks\":[{\"type\":\"section\",\"text\":{\"type\":\"mrkdwn\",\"text\":\"*Sessions*\"}},{\"type\":\"divider\"},{\"type\":\"section\",\"text\":{\"type\":\"mrkdwn\",\"text\":\"*01/01/2019*\"}},{\"type\":\"section\",\"text\":{\"type\":\"mrkdwn\",\"text\":\"**\\n04:30 - 05:30\\nRory\\n\"}},{\"type\":\"section\",\"text\":{\"type\":\"mrkdwn\",\"text\":\"**\\n04:45 - 05:45\\nRory\\n\"}},{\"type\":\"divider\"},{\"type\":\"section\",\"text\":{\"type\":\"mrkdwn\",\"text\":\"*05/01/2019*\"}},{\"type\":\"section\",\"text\":{\"type\":\"mrkdwn\",\"text\":\"**\\n04:30 - 05:30\\nRory in Everest\\nCurrent number of attendees: 0\"},\"accessory\":{\"type\":\"button\",\"text\":{\"type\":\"plain_text\",\"text\":\"Attend\"},\"value\":null}}]}";
            responseBody.Should().Be(expectedJson);
            webClient.ResponseHeaders["Content-Type"].Should().Be("application/json");
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
                        Duration = 1,
                        Attendees = new List<string>(),
                        Host = "Rory",
                        Location = "Everest"
                    },
                    new PresentableWorkshop
                    {
                        Time = new DateTimeOffset(2019, 1, 1, 1, 1, 1, TimeSpan.Zero),
                        Duration = 60,
                        Attendees = new List<string>(),
                        Host = "Rory",
                        Location = "Everest"
                    },
                    new PresentableWorkshop
                    {
                        Time = new DateTimeOffset(2015, 1, 1, 1, 1, 1, TimeSpan.Zero),
                        Duration = 120,
                        Attendees = new List<string>(),
                        Host = "Rory",
                        Location = "Everest"
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
    public class AlwaysTwoShowcasesAndOneWorkshop : IGetWorkshops
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
                        Duration = 60,
                        Attendees = new List<string>(),
                        Host = "Rory",
                        Location = "Everest"
                    },
                    new PresentableWorkshop
                    {
                        Type = "Showcase",
                        Time = new DateTimeOffset(2019, 1, 1, 4, 45, 1, TimeSpan.Zero),
                        Duration = 60,
                        Attendees = new List<string>(),
                        Host = "Rory",
                        Location = "Everest"
                    },
                    new PresentableWorkshop
                    {
                        Type = "Workshop",
                        Time = new DateTimeOffset(2019, 1, 5, 4, 30, 1, TimeSpan.Zero),
                        Duration = 60,
                        Attendees = new List<string>(),
                        Host = "Rory",
                        Location = "Everest"
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