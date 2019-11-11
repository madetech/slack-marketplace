using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using CryptoTechProject.Boundary;
using CryptoTechProject.Domain;
using FluentAssertions;
using NUnit.Framework;
using FluentSim;
using Newtonsoft.Json;

namespace CryptoTechProject.Tests
{
    [TestFixture]
    public class AirtableGatewayUnitTest
    {
        private string AIRTABLE_API_KEY = "111";
        private string TABLE_ID = "2";
        private string AIRTABLE_URL = "http://localhost:8080/";
        AirtableSimulator airtableSimulator;


        [SetUp]
        public void SetUp()
        {
            airtableSimulator = new AirtableSimulator();
            airtableSimulator.Start();
        }

        [TearDown]
        public void TearDown()
        {
            airtableSimulator.Stop();
        }

        [Test]
        public void CanGetAWorkshop()
        {
            var expectedResponse = new AirtableResponseBuilder()
                .AddRecord(
                    "rec0d7vSGRLxzbUNz",
                    new DateTime(2019, 8, 22, 8, 25, 28)
                ).WithName("Coding Black Females - Code Dojo")
                .WithHost("Made Tech")
                .WithCategories("Meetup")
                .WithTime(2019, 9, 18, 17, 0, 0)
                .WithDurationInSeconds(10800)
                .WithLocation("Made Tech O'Meara")
                .WithSessionType("Code Dojo")
                .Build();

            airtableSimulator.SetUpAll(expectedResponse, TABLE_ID, AIRTABLE_API_KEY);


            AirtableGateway airtableGateway = new AirtableGateway(AIRTABLE_URL, AIRTABLE_API_KEY, TABLE_ID);
            var workshops = airtableGateway.All();

            DateTime time = new DateTime(2019, 09, 18, 17, 00, 0);

            var firstWorkshop = workshops[0];

            firstWorkshop.name.Should().Be("Coding Black Females - Code Dojo");
            firstWorkshop.host.Should().Be("Made Tech");
            firstWorkshop.time.Should().Be(time);
            firstWorkshop.location.Should().Be("Made Tech O'Meara");
            firstWorkshop.duration.Should().Be(180);
            firstWorkshop.type.Should().Be("Code Dojo");
        }

        [Test]
        public void CanGetTwoWorkshops()
        {
            var expectedResponse = new AirtableResponseBuilder()
                .AddRecord(
                    "rec4rdaOkafgV1Bqm",
                    new DateTime(2019, 8, 22, 8, 25, 28)
                ).WithName("Team Performance: Team Agile-Lean maturity 'measures' in practice (at DfE and Hackney)")
                .WithHost("Barry")
                .WithCategories("Delivery")
                .WithTime(2019, 10, 18, 13, 0, 0)
                .WithDurationInSeconds(3600)
                .WithLocation("Everest, 2nd Foor")
                .WithSessionType("Seminar")
                .AddRecord(
                    "reca7W6WxWubIR7CK",
                    new DateTime(2019, 8, 27, 5, 24, 25)
                )
                .WithName("Account Leadership - Roles & Responsibilities")
                .WithHost("Rory")
                .WithCategories("Sales", "Workshop", "Life Skills", "Business")
                .WithTime(2019, 10, 18, 14, 30, 0)
                .WithDurationInSeconds(3600)
                .WithLocation("Everest")
                .WithSessionType("Workshop")
                .Build();

            airtableSimulator.SetUpAll(expectedResponse, TABLE_ID, AIRTABLE_API_KEY);

            AirtableGateway airtableGateway = new AirtableGateway(AIRTABLE_URL, AIRTABLE_API_KEY, TABLE_ID);
            var workshops = airtableGateway.All();

            workshops[0].name.Should().Be("Team Performance: Team Agile-Lean maturity 'measures' in practice (at DfE and Hackney)");
            workshops[1].name.Should().Be("Account Leadership - Roles & Responsibilities");
        }
        
        [Test]
        public void CanFindWorkshopByID()
        {
            var expectedResponse = new AirtableResponseBuilder()
                .AddRecord(
                    "rec4rdaOkafgV1Bqm",
                    new DateTime(2019, 8, 22, 8, 25, 28)
                ).WithName("Team Performance: Team Agile-Lean maturity 'measures' in practice (at DfE and Hackney)")
                .WithHost("Barry")
                .WithCategories("Delivery")
                .WithTime(2019, 10, 18, 13, 0, 0)
                .WithDurationInSeconds(3600)
                .WithLocation("Everest, 2nd Foor")
                .WithSessionType("Seminar")
                .Build();

            airtableSimulator.SetUpFind(TABLE_ID, AIRTABLE_API_KEY, expectedResponse.Records[0], "rec4rdaOkafgV1Bqm");

            AirtableGateway airtableGateway = new AirtableGateway(AIRTABLE_URL, AIRTABLE_API_KEY, TABLE_ID);
            Workshop firstWorkshop = airtableGateway.Find("rec4rdaOkafgV1Bqm");

            DateTime time = new DateTime(2019, 10, 18, 13, 0, 0);

            firstWorkshop.name.Should().Be("Team Performance: Team Agile-Lean maturity 'measures' in practice (at DfE and Hackney)");
            firstWorkshop.host.Should().Be("Barry");
            firstWorkshop.time.Should().Be(time);
            firstWorkshop.duration.Should().Be(3600/60);
            firstWorkshop.location.Should().Be("Everest, 2nd Foor");
            firstWorkshop.type.Should().Be("Seminar");
        }
        
        
        [Test]
        public void SendsCorrectAirtableRequest()
        {
            var expectedResponse = new AirtableResponseBuilder()
                .AddRecord(
                    "rec4rdaOkafgV1Bqm",
                    new DateTime(2019, 8, 22, 8, 25, 28)
                ).WithName("Team Performance: Team Agile-Lean maturity 'measures' in practice (at DfE and Hackney)")
                .WithHost("Barry")
                .WithCategories("Delivery")
                .WithTime(2019, 10, 18, 13, 0, 0)
                .WithDurationInSeconds(3600)
                .WithLocation("Everest, 2nd Foor")
                .WithSessionType("Seminar")
                .Build();

            airtableSimulator.SetUpFind(TABLE_ID, AIRTABLE_API_KEY, expectedResponse.Records[0], "rec4rdaOkafgV1Bqm");

            AirtableGateway airtableGateway = new AirtableGateway(AIRTABLE_URL, AIRTABLE_API_KEY, TABLE_ID);
            Workshop firstWorkshop = airtableGateway.Find("rec4rdaOkafgV1Bqm");

            DateTime time = new DateTime(2019, 10, 18, 13, 0, 0);

            firstWorkshop.name.Should().Be("Team Performance: Team Agile-Lean maturity 'measures' in practice (at DfE and Hackney)");
            firstWorkshop.host.Should().Be("Barry");
            firstWorkshop.time.Should().Be(time);
            firstWorkshop.duration.Should().Be(3600/60);
            firstWorkshop.location.Should().Be("Everest, 2nd Foor");
            firstWorkshop.type.Should().Be("Seminar");
            
            var headers = airtableSimulator.simulator.ReceivedRequests[0].Headers;
            
            headers["Authorization"].Should().Be("Bearer 111");
            headers["Content-type"].Should().Be("application/json");
        }
        
        [Test]
        public void CanAddAttendeeToAWorkshopWithNoAttendees()
        {
            airtableSimulator.SetUpSave(TABLE_ID, AIRTABLE_API_KEY);
            
            Workshop workshopParameter = new Workshop()
            {
                id = "aaa",
                attendees = new List<string>(){"Maria"} 
            };
            
            AirtableGateway airtableGateway = new AirtableGateway(AIRTABLE_URL, AIRTABLE_API_KEY, TABLE_ID);
            airtableGateway.Update(workshopParameter);
            var requests = airtableSimulator.simulator.ReceivedRequests;
           
            var receivedRequest = requests[0].BodyAs<AirtableRequest>();

            receivedRequest.Records[0].Fields.Attendees[0].Should().Be("Maria");
            receivedRequest.Typecast.Should().Be(true);
            requests[0].ContentType.Should().Be("application/json");
            requests[0].Headers["Authorization"].Should().Be("Bearer " + AIRTABLE_API_KEY);
        }
        
        [Test]
        public void CanOnlyGetWorkshopsWithDates()
        {
            var expectedResponse = new AirtableResponseBuilder()
                .AddRecord(
                    "rec4rdaOkafgV1Bqm",
                    new DateTime(2019, 8, 22, 8, 25, 28)
                ).WithName("Team Performance: Team Agile-Lean maturity 'measures' in practice (at DfE and Hackney)")
                .WithHost("Barry")
                .WithCategories("Delivery")
                .WithTime(2019, 10, 18, 13, 0, 0)
                .WithDurationInSeconds(3600)
                .WithLocation("Everest, 2nd Foor")
                .WithSessionType("Seminar")
                .AddRecord(
                    "reca7W6WxWubIR7CK",
                    new DateTime(2019, 8, 27, 5, 24, 25)
                )
                .WithName("Account Leadership - Roles & Responsibilities")
                .WithHost("Rory")
                .WithCategories("Sales", "Workshop", "Life Skills", "Business")
                .WithTime(2019, 10, 18, 14, 30, 0)
                .WithDurationInSeconds(3600)
                .WithLocation("Everest")
                .WithSessionType("Workshop")
                .Build();

            airtableSimulator.SetUpAll(expectedResponse, TABLE_ID, AIRTABLE_API_KEY);

            AirtableGateway airtableGateway = new AirtableGateway(AIRTABLE_URL, AIRTABLE_API_KEY, TABLE_ID);
            var workshops = airtableGateway.All();

            workshops[0].name.Should().Be("Team Performance: Team Agile-Lean maturity 'measures' in practice (at DfE and Hackney)");
            workshops[1].name.Should().Be("Account Leadership - Roles & Responsibilities");
        }
        
    }
}