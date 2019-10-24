using System.Collections.Generic;
using CryptoTechProject.Domain;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace CryptoTechProject.Tests
{
    public class ViewWorkshopsTest
    {
        [TestFixture]
        public class WorkshopsUnitTest : IWorkshopsGateway
        {
            private List<Workshop> _workshops;

            [SetUp]
            public void Setup()
            {
                _workshops = new List<Workshop>();
            }

            [Test]
            public void CanGetNoWorkshop()
            {
                var response = new GetWorkshops(this).Execute();
                Assert.IsEmpty(response.PresentableWorkshops);
            }

            [Test]
            public void CanGetAWorkshop()
            {
                _workshops.Add(new Workshop());
                var response = new GetWorkshops(this).Execute();
                Assert.IsNotEmpty(response.PresentableWorkshops);
            }

            public List<Workshop> All()
            {
                return _workshops;
            }

            public void Save(Workshop workshop)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
