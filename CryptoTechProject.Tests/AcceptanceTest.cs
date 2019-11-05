using System;
using System.Collections.Generic;
using CryptoTechProject.Boundary;
using CryptoTechProject.Domain;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace CryptoTechProject.Tests
{
    [TestFixture]
    public class AcceptanceTest
    {
        private string AIRTABLE_API_KEY = "111";
        private string TABLE_ID = "2";
        private string AIRTABLE_URL = "http://localhost:8080/";

        AirtableSimulator airtable;
        private AirtableGateway _gateway;

        private GetWorkshopsResponse GetWorkshops()
        {
            GetWorkshops getWorkshops = new GetWorkshops(_gateway);
            GetWorkshopsResponse response = getWorkshops.Execute();
            return response;
        }

        [SetUp]
        public void SetUp()
        {
            airtable = new AirtableSimulator();
            airtable.Start();
            _gateway = new AirtableGateway(AIRTABLE_URL, AIRTABLE_API_KEY, TABLE_ID);
        }

        [TearDown]
        public void TearDown()
        {
            airtable.Stop();
        }

        [Test]
        public void CanGetTwoAirtableWorkshops()
        {
            var getRecordsResponse = new AirtableResponseBuilder()
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

            airtable.SetUpAll(getRecordsResponse, TABLE_ID, AIRTABLE_API_KEY);
            
            var response = GetWorkshops();

            DateTime sourceDate = new DateTime(2019, 10, 18, 14, 00, 0);
            DateTimeOffset time = new DateTimeOffset(sourceDate,
                TimeZoneInfo.FindSystemTimeZoneById("Europe/London").GetUtcOffset(sourceDate));


            DateTime sourceDate2 = new DateTime(2019, 10, 18, 15, 30, 0);
            DateTimeOffset time2 = new DateTimeOffset(sourceDate2,
                TimeZoneInfo.FindSystemTimeZoneById("Europe/London").GetUtcOffset(sourceDate2));

            PresentableWorkshop[] presentableWorkshops = response.PresentableWorkshops;

            var theFirstWorkshop = presentableWorkshops[0];
            var theSecondWorkshop = presentableWorkshops[1];
            
            theFirstWorkshop.Name.Should().Be("Team Performance: Team Agile-Lean maturity 'measures' in practice (at DfE and Hackney)");
            theFirstWorkshop.Host.Should().Be("Barry");
            theFirstWorkshop.Time.Should().Be(time);
            theFirstWorkshop.Location.Should().Be("Everest, 2nd Foor");        
            theFirstWorkshop.Duration.Should().Be(60);
            theFirstWorkshop.Type.Should().Be("Seminar");
            
            theSecondWorkshop.Name.Should().Be("Account Leadership - Roles & Responsibilities");
            theSecondWorkshop.Host.Should().Be("Rory");
            theSecondWorkshop.Time.Should().Be(time2);
            theSecondWorkshop.Location.Should().Be("Everest");
            theSecondWorkshop.Duration.Should().Be(60);
            theSecondWorkshop.Type.Should().Be("Workshop");
        }

        [Test]
        public void AddsUserToAirtableTable()
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
                .WithAttendees(new List<string>())
                .Build();

            airtable.SetUpFind(TABLE_ID, AIRTABLE_API_KEY, expectedResponse.Records[0], "ID000");
            airtable.SetUpSave(TABLE_ID, AIRTABLE_API_KEY);
            
            ToggleWorkshopAttendance attend = new ToggleWorkshopAttendance(_gateway, _gateway);
            ToggleWorkshopAttendanceRequest payload = new ToggleWorkshopAttendanceRequest();
            
            payload.User = "Maria";
            payload.WorkshopId = "ID000";
            attend.Execute(payload);

            var requests = airtable.simulator.ReceivedRequests;
            var sentUser = requests[1].BodyAs<AirtableRequest>();
            Fields fields = sentUser.Records[0].Fields;
            
            fields.Attendees[0].Should().Be("Maria");
        }
        
        [Test]
        public void RemovesUserFromAirtableTable()
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
                .WithAttendees(new List<string>(){"Maria", "Kat"})
                .Build();

            airtable.SetUpFind(TABLE_ID, AIRTABLE_API_KEY, expectedResponse.Records[0], "ID000");
            airtable.SetUpSave(TABLE_ID, AIRTABLE_API_KEY);

            ToggleWorkshopAttendance attend = new ToggleWorkshopAttendance(_gateway, _gateway);
            ToggleWorkshopAttendanceRequest payload = new ToggleWorkshopAttendanceRequest();
            
            payload.User = "Maria";
            payload.WorkshopId = "ID000";
            attend.Execute(payload);

            var requests = airtable.simulator.ReceivedRequests;
            var sentUser = requests[1].BodyAs<AirtableRequest>();
            Fields fields = sentUser.Records[0].Fields;

            fields.Attendees.Should().NotContain("Maria");
        }
    }
}