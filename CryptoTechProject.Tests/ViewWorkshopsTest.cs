using CryptoTechProject.Domain;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace CryptoTechProject.Tests
{
    public class ViewWorkshopsTest
    {
        [TestFixture]
        public class ViewWorkshopsUnitTest
        {
            [Test]
            public void ViewWorkshops_WhenNoWorkshops_ReturnsEmptyHash(){
                Workshop workshop = new Workshop();
                Gateway viewWorkshopsGateway = new Gateway(workshop);
                ViewWorkshops viewWorkshops = new ViewWorkshops(viewWorkshopsGateway);
                Assert.AreEqual(workshop, viewWorkshops.Execute());                
            }
            
            [Test]
            [Ignore("")]
            public void ViewWorkshops_WhenOneWorkshop_ReturnsWorkshopJObject(){
                Workshop workshop = new Workshop();
                Gateway viewWorkshopsGateway = new Gateway( workshop);
                ViewWorkshops viewWorkshops = new ViewWorkshops(viewWorkshopsGateway);
                JObject json = new JObject();
                //
                json.Add("name","Duplication");
                Assert.AreEqual(json, viewWorkshops.Execute());                
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