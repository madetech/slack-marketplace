using System;
using CryptoTechProject.Boundary;
using NUnit.Framework;

namespace CryptoTechProject.Tests
{
    [TestFixture]
    public class HardCodedWorkshopsGatewayUnitTest
    {
        [Test]
        public void CanGetHardcodedWorkshops(){
            HardCodedWorkshopsGateway hardCodedWorkshopsGateway = new HardCodedWorkshopsGateway();
            DateTime sourceDate = new DateTime(2008, 5, 1, 8, 30, 0);
            DateTimeOffset time = new DateTimeOffset(sourceDate, 
                TimeZoneInfo.FindSystemTimeZoneById("Europe/London").GetUtcOffset(sourceDate)); 
            
            Assert.AreEqual("Coding Black Females - Code Dojo", hardCodedWorkshopsGateway.All()[0].name);
            Assert.AreEqual("Made Tech",hardCodedWorkshopsGateway.All()[0].host);
            Assert.AreEqual(time, hardCodedWorkshopsGateway.All()[0].time);
            Assert.AreEqual("Made Tech O'Meara",hardCodedWorkshopsGateway.All()[0].location);
            Assert.AreEqual(10800/60,hardCodedWorkshopsGateway.All()[0].duration);
            Assert.AreEqual("Code Dojo",hardCodedWorkshopsGateway.All()[0].type);


        }
    }
}