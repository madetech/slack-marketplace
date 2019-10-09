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
    }
}