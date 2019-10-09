using System;
using CryptoTechProject.Domain;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace CryptoTechProject.Tests
{

    [TestFixture]
    public class AcceptanceTest
    {
        [Test]
        public void WhenNoWorkshops_ViewWorkshops_ReturnsEmptyHash(){
            Workshop workshop = new Workshop();
            Gateway viewWorkshopsGateway = new Gateway(workshop);
            ViewWorkshops viewWorkshops = new ViewWorkshops(viewWorkshopsGateway);
            Assert.AreEqual(workshop, viewWorkshops.Execute());                               
        }
        
        

    }
}