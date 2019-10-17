using System;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using CryptoTechProject.Domain;
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
                .WithLocation("Made Tech O'Meara'")
                .WithSessionType("Code Dojo")
                .Build();

            airtableSimulator.SetUp(TABLE_ID, AIRTABLE_API_KEY, expectedResponse);


            AirtableGateway airtableGateway = new AirtableGateway(AIRTABLE_URL, AIRTABLE_API_KEY, TABLE_ID);
            var workshops = airtableGateway.All();

            DateTime time = new DateTime(2019, 09, 18, 17, 00, 0);

            Assert.True(workshops[0].name.Contains("Coding Black Females - Code Dojo"));
            Assert.True(workshops[0].host.Contains("Made Tech"));
            Assert.AreEqual(workshops[0].time, time);
            Assert.True(workshops[0].location.Contains("O'Meara"));
            Assert.AreEqual(workshops[0].duration, 180);
            Assert.True(workshops[0].type.Contains("Code Dojo"));

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

            airtableSimulator.SetUp(TABLE_ID, AIRTABLE_API_KEY, expectedResponse);

            AirtableGateway airtableGateway = new AirtableGateway(AIRTABLE_URL, AIRTABLE_API_KEY, TABLE_ID);
            var workshops = airtableGateway.All();

            Assert.True(workshops[0].name.Contains("Team Performance:"));
            Assert.True(workshops[1].name.Contains("Account Leadership"));
        }
    }
}