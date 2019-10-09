using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace CryptoTechProject.Tests
{
    public class ViewWorkshops_Test
    {
        [TestFixture]
        public class AcceptanceTest
        {
            [Test]
            public void WhenNoWorkshops_ViewWorkshops_ReturnsEmptyHash(){
                Gateway gateway = new Gateway("{}");
                ViewWorkshops viewWorkshops = new ViewWorkshops(gateway);
                Assert.AreEqual(new JObject(), viewWorkshops.Execute());                
            }

        }
    }
}