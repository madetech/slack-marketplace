using System.Collections.Generic;
using CryptoTechProject.Domain;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace CryptoTechProject.Tests
{
    public class ViewWorkshopsTest
    {
        [TestFixture]

        
        public class GetWorkshopsUnitTest: IGetWorkshopsGateway
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
                Assert.AreEqual(0, response.PresentableWorkshops.Length);
            }
            
            [Test]
            public void CanGetAWorkshop()
            {
                _workshops.Add(new Workshop());
                var response = new GetWorkshops(this).Execute(); 
                Assert.AreEqual(1, response.PresentableWorkshops.Length);
            }

            public List<Workshop> All()
            {
                return _workshops;
            }
        }
        
        
    }
}
/*

"{\"id\":\"rec0d7vSGRLxzbUNz\"," +
"Name\":\"Coding Black Females - Code Dojo\"," +
"\"Host\":\"Made Tech\"," +
"\"Location\":\"Made Tech O'Meara\"," +
"\"Time\":\"2019-09-18T17:00:00.000Z\"," +
"\"Duration\":10800," +
"\"Session Type\":\"Code Dojo\"," +
"\"Categories\":[" +
"\"Meetup\"]}," +
"\"createdTime\":\"2019-09-18T09:29:25.000Z\" }"*/