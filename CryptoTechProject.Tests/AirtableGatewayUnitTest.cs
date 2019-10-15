using System;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using CryptoTechProject.Domain;
using NUnit.Framework;
using FluentSim;

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
                "{\"records\":[{\"id\":\"rec4rdaOkafgV1Bqm\",\"fields\":{\"Name\":\"Team Performance: Team Agile-Lean maturity 'measures' in practice (at DfE and Hackney)\",\"Host\":\"Barry\",\"Location\":\"Everest, 2nd Foor\",\"Time\":\"2019-10-18T13:00:00.000Z\",\"Duration\":3600,\"Session Type\":\"Seminar\",\"Categories\":[\"Delivery\"]},\"createdTime\":\"2019-08-22T08:25:28.000Z\"}]}";
            simulator.Get("/v0/"+TABLE_ID+"/Marketplace" ).WithParameter("api_key", AIRTABLE_API_KEY).Responds(jsonString);
            
            AirtableGateway airtableGateway = new AirtableGateway(AIRTABLE_URL, AIRTABLE_API_KEY, TABLE_ID);
            var workshops = airtableGateway.All();
            var url =
                "https://api.airtable.com/v0/apppGaPfIFW8nmf4T/Marketplace?maxRecords=1&view=Upcoming&api_key=keyFfgviAZnwbXCxZ";
            /*
             * get gateway to go to the server
             * get response from server
             * translate response from json to domain object
             * return that object
             * check if object 
             */
           
            
            Assert.True(workshops[0].name.Contains("Team Performance: Team Agile-Lean maturity"));
            
        }
     
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