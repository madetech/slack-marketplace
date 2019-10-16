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
        public void CanGetHardcodedWorkshops()
        {
            HardCodedWorkshopsGateway hardCodedWorkshopsGateway = new HardCodedWorkshopsGateway();
            GetWorkshops getWorkshops = new GetWorkshops(hardCodedWorkshopsGateway);
            GetWorkshopsResponse response = getWorkshops.Execute();
            DateTime sourceDate = new DateTime(2019, 10, 18, 14, 00, 0);
            DateTimeOffset time = new DateTimeOffset(sourceDate,
                TimeZoneInfo.FindSystemTimeZoneById("Europe/London").GetUtcOffset(sourceDate));

            Assert.AreEqual("Team Performance: Team Agile-Lean maturity 'measures' in practice (at DfE and Hackney)",
                hardCodedWorkshopsGateway.All()[0].name);
            Assert.AreEqual("Barry", hardCodedWorkshopsGateway.All()[0].host);
            Assert.AreEqual(time, hardCodedWorkshopsGateway.All()[0].time);
            Assert.AreEqual("Everest", hardCodedWorkshopsGateway.All()[0].location);
            Assert.AreEqual(60, hardCodedWorkshopsGateway.All()[0].duration);
            Assert.AreEqual("Seminar", hardCodedWorkshopsGateway.All()[0].type);
        }

        [Test]
        public void CanGetTwoHardcodedWorkshops()
        {
            HardCodedWorkshopsGateway hardCodedWorkshopsGateway = new HardCodedWorkshopsGateway();
            GetWorkshops getWorkshops = new GetWorkshops(hardCodedWorkshopsGateway);
            GetWorkshopsResponse response = getWorkshops.Execute();
            DateTime sourceDate = new DateTime(2019, 10, 18, 14, 00, 0);
            DateTimeOffset time = new DateTimeOffset(sourceDate,
                TimeZoneInfo.FindSystemTimeZoneById("Europe/London").GetUtcOffset(sourceDate));

            DateTime sourceDate2 = new DateTime(2019, 10, 18, 15, 30, 0);
            DateTimeOffset time2 = new DateTimeOffset(sourceDate2,
                TimeZoneInfo.FindSystemTimeZoneById("Europe/London").GetUtcOffset(sourceDate2));

            Assert.AreEqual("Team Performance: Team Agile-Lean maturity 'measures' in practice (at DfE and Hackney)",
                hardCodedWorkshopsGateway.All()[0].name);
            Assert.AreEqual("Barry", hardCodedWorkshopsGateway.All()[0].host);
            Assert.AreEqual(time, hardCodedWorkshopsGateway.All()[0].time);
            Assert.AreEqual("Everest", hardCodedWorkshopsGateway.All()[0].location);
            Assert.AreEqual(60, hardCodedWorkshopsGateway.All()[0].duration);
            Assert.AreEqual("Seminar", hardCodedWorkshopsGateway.All()[0].type);

            Assert.AreEqual("Account Leadership - Roles & Responsibilities", response.PresentableWorkshops[1].Name);
            Assert.AreEqual("Rory", response.PresentableWorkshops[1].Host);
            Assert.AreEqual(time2, response.PresentableWorkshops[1].Time);
            Assert.AreEqual("Everest", response.PresentableWorkshops[1].Location);
            Assert.AreEqual(60, response.PresentableWorkshops[1].Duration);
            Assert.AreEqual("Workshop", response.PresentableWorkshops[1].Type);
        }
    }
}