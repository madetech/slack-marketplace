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
            simulator.Get("/v0/"+TABLE_ID+"/Marketplace" ).WithParameter("api_key", AIRTABLE_API_KEY).Responds(jsonString);
            
            AirtableGateway airtableGateway = new AirtableGateway(AIRTABLE_URL, AIRTABLE_API_KEY, TABLE_ID);
            var workshops = airtableGateway.All();
            var url =
                "https://api.airtable.com/v0/apppGaPfIFW8nmf4T/Marketplace?maxRecords=1&view=Upcoming&api_key=keyFfgviAZnwbXCxZ";

            // serialise workshops back to string?
            
            Assert.True(workshops[0].name.Contains("Coding Black Females - Code Dojo"));
            
        }
     
        [Ignore("I don't need to justify myself to you")]
        [Test]
        public void CanGetADifferentWorkshop()
        {
           
           
            string jsonString =
                "{\"records\":[{\"id\":\"rec0d7vSGRLxzbUNz\",\"fields\":{\"Name\":\"Coding Black Females - Code Dojo\",\"Host\":\"Made Tech\",\"Location\":\"Made Tech O'Meara\",\"Time\":\"2019-09-18T17:00:00.000Z\",\"Duration\":10800,\"Session Type\":\"Code Dojo\",\"Categories\":[\"Meetup\"]},\"createdTime\":\"2019-09-18T09:29:25.000Z\"}]}";
            simulator.Get("/v0/"+TABLE_ID+"/Marketplace" ).WithParameter("api_key", AIRTABLE_API_KEY).Responds(jsonString);
            
            AirtableGateway airtableGateway = new AirtableGateway(AIRTABLE_URL, AIRTABLE_API_KEY, TABLE_ID);
            var workshops = airtableGateway.All();
            
            Assert.True(workshops[0].name.Contains("Coding Black Females - Code Dojo"));
            
        }

    }
    
    
}