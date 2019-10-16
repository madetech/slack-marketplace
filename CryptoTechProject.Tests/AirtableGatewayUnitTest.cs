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
        FluentSimulator simulator;


        [SetUp]
        public void SetUp()
        {
            simulator = new FluentSimulator("http://localhost:8080/");
            simulator.Start();
        }

        [TearDown]
        public void TearDown()
        {
            //Stop the simulator
            simulator.Stop();
        }

        [Test]
        public void CanGetAWorkshop()
        {
            string jsonString =
                "{\"records\":[{\"id\":\"rec0d7vSGRLxzbUNz\",\"fields\":{\"Name\":\"Coding Black Females - Code Dojo\",\"Host\":\"Made Tech\",\"Location\":\"Made Tech O'Meara\",\"Time\":\"2019-09-18T17:00:00.000Z\",\"Duration\":10800,\"Session Type\":\"Code Dojo\",\"Categories\":[\"Meetup\"]},\"createdTime\":\"2019-09-18T09:29:25.000Z\"}]}";
            simulator.Get("/v0/" + TABLE_ID + "/Marketplace").WithParameter("api_key", AIRTABLE_API_KEY)
                .Responds(jsonString);

            AirtableGateway airtableGateway = new AirtableGateway(AIRTABLE_URL, AIRTABLE_API_KEY, TABLE_ID);
            var workshops = airtableGateway.All();

            DateTime sourceDate = new DateTime(2019, 09, 18, 18, 00, 0);
            DateTimeOffset time = new DateTimeOffset(sourceDate,
                TimeZoneInfo.FindSystemTimeZoneById("Europe/London").GetUtcOffset(sourceDate));

            Assert.True(workshops[0].name.Contains("Coding Black Females - Code Dojo"));
            Assert.AreEqual(workshops[0].time, time);
        }

        [Test]
        public void CanGetTwoWorkshops()
        {
            string jsonString =
                "{\"records\":[{\"id\":\"rec4rdaOkafgV1Bqm\",\"fields\":{\"Name\":\"Team Performance: Team Agile-Lean maturity 'measures' in practice (at DfE and Hackney)\",\"Host\":\"Barry\",\"Location\":\"Everest, 2nd Foor\",\"Time\":\"2019-10-18T13:00:00.000Z\",\"Duration\":3600,\"Session Type\":\"Seminar\",\"Categories\":[\"Delivery\"]},\"createdTime\":\"2019-08-22T08:25:28.000Z\"},{\"id\":\"reca7W6WxWubIR7CK\",\"fields\":{\"Name\":\"Account Leadership - Roles & Responsibilities\",\"Host\":\"Rory\",\"Location\":\"Everest\",\"Time\":\"2019-10-18T14:30:00.000Z\",\"Duration\":216000,\"Session Type\":\"Workshop\",\"Categories\":[\"Sales\",\"Workshop\",\"Life Skills\",\"Business\"]},\"createdTime\":\"2019-08-27T05:24:25.000Z\"}]}";
            simulator.Get("/v0/" + TABLE_ID + "/Marketplace").WithParameter("api_key", AIRTABLE_API_KEY)
                .Responds(jsonString);

            AirtableGateway airtableGateway = new AirtableGateway(AIRTABLE_URL, AIRTABLE_API_KEY, TABLE_ID);
            var workshops = airtableGateway.All();

            Assert.True(workshops[0].name.Contains("Team Performance:"));
            Assert.True(workshops[1].name.Contains("Account Leadership"));
        }
    }
}