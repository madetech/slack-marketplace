using System;
using CryptoTechProject.Boundary;
using CryptoTechProject.Domain;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace CryptoTechProject.Tests
{

    [TestFixture]
    public class AcceptanceTest
    {
        [Test]
        public void CanGetHardcodedWorkshops(){
            HardCodedWorkshopsGateway hardCodedWorkshopsGateway = new HardCodedWorkshopsGateway();
            GetWorkshops getWorkshops = new GetWorkshops(hardCodedWorkshopsGateway);
            GetWorkshopsResponse response = getWorkshops.Execute();
            DateTime sourceDate = new DateTime(2008, 5, 1, 8, 30, 0);
            DateTimeOffset time = new DateTimeOffset(sourceDate, 
                TimeZoneInfo.FindSystemTimeZoneById("Europe/London").GetUtcOffset(sourceDate)); 
            
            Assert.AreEqual("Coding Black Females - Code Dojo",response.PresentableWorkshops[0].Name);
            Assert.AreEqual("Made Tech",response.PresentableWorkshops[0].Host);
            Assert.AreEqual(time, response.PresentableWorkshops[0].Time);
            Assert.AreEqual("Made Tech O'Meara",response.PresentableWorkshops[0].Location);
            Assert.AreEqual(10800/60,response.PresentableWorkshops[0].Duration);
            Assert.AreEqual("Code Dojo",response.PresentableWorkshops[0].Type);
            
        }
        
        [Test]
        public void CanGetTwoHardcodedWorkshops(){
            HardCodedWorkshopsGateway hardCodedWorkshopsGateway = new HardCodedWorkshopsGateway();
            GetWorkshops getWorkshops = new GetWorkshops(hardCodedWorkshopsGateway);
            GetWorkshopsResponse response = getWorkshops.Execute();
            DateTime sourceDate = new DateTime(2008, 5, 1, 8, 30, 0);
            DateTimeOffset time = new DateTimeOffset(sourceDate, 
                TimeZoneInfo.FindSystemTimeZoneById("Europe/London").GetUtcOffset(sourceDate)); 
            
            DateTime sourceDate2 = new DateTime(2019, 10, 18, 15, 30, 0);
            DateTimeOffset time2 = new DateTimeOffset(sourceDate2, 
                TimeZoneInfo.FindSystemTimeZoneById("Europe/London").GetUtcOffset(sourceDate2)); 

            Assert.AreEqual("Coding Black Females - Code Dojo",response.PresentableWorkshops[0].Name);
            Assert.AreEqual("Made Tech",response.PresentableWorkshops[0].Host);
            Assert.AreEqual(time, response.PresentableWorkshops[0].Time);
            Assert.AreEqual("Made Tech O'Meara",response.PresentableWorkshops[0].Location);
            Assert.AreEqual(10800/60,response.PresentableWorkshops[0].Duration);
            Assert.AreEqual("Code Dojo",response.PresentableWorkshops[0].Type);
            
            Assert.AreEqual("Account Leadership - Roles & Responsibilities",response.PresentableWorkshops[1].Name);
            Assert.AreEqual("Rory",response.PresentableWorkshops[1].Host);
            Assert.AreEqual(time2, response.PresentableWorkshops[1].Time);
            Assert.AreEqual("Everest",response.PresentableWorkshops[1].Location);
            Assert.AreEqual(60,response.PresentableWorkshops[1].Duration);
            Assert.AreEqual("Workshop",response.PresentableWorkshops[1].Type);
        }
    }
}