using System;
using CryptoTechProject.Boundary;
using NUnit.Framework;

namespace CryptoTechProject.Tests
{
    [TestFixture]
    public class HardCodedWorkshopsGatewayUnitTest
    {
        [Test]
        public void CanGetHardcodedWorkshops()
        {
            HardCodedWorkshopsGateway hardCodedWorkshopsGateway = new HardCodedWorkshopsGateway();
            DateTime time = new DateTime(2019, 10, 18, 14, 00, 0);

            Assert.AreEqual("Team Performance: Team Agile-Lean maturity 'measures' in practice (at DfE and Hackney)",
                hardCodedWorkshopsGateway.All()[0].name);
            Assert.AreEqual("Barry", hardCodedWorkshopsGateway.All()[0].host);
            Assert.AreEqual(time, hardCodedWorkshopsGateway.All()[0].time);
            Assert.AreEqual("Everest", hardCodedWorkshopsGateway.All()[0].location);
            Assert.AreEqual(60, hardCodedWorkshopsGateway.All()[0].duration);
            Assert.AreEqual("Seminar", hardCodedWorkshopsGateway.All()[0].type);
        }
    }
}