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
                Gateway viewWorkshopsGateway = new Gateway("");
                ViewWorkshops viewWorkshops = new ViewWorkshops(viewWorkshopsGateway);
                Assert.AreEqual(new JObject(), viewWorkshops.Execute());                
            }
            
            [Test]
            public void ViewWorkshops_WhenOneWorkshop_ReturnsWorkshopJObject(){
                Gateway viewWorkshopsGateway = new Gateway( "{\"name\":\"Duplicaton\"}");
                ViewWorkshops viewWorkshops = new ViewWorkshops(viewWorkshopsGateway);
                JObject json = new JObject();
                //Workshop workshop = new Workshop();
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