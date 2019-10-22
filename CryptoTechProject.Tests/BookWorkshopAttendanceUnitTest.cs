using CryptoTechProject.Boundary;
using NUnit.Framework;

namespace CryptoTechProject.Tests
{
    [TestFixture]
    public class BookWorkshopAttendanceUnitTest
    {
        [Test]
        public void ConfirmsUserAttendance()
        {
            HardCodedWorkshopsGateway gateway = new HardCodedWorkshopsGateway();
            BookWorkshopAttendance attend = new BookWorkshopAttendance(gateway);
            FunctionalPayload payload = new FunctionalPayload();
            payload.User = "Maria";
            payload.Workshop = "Workshop Name";
            Assert.AreEqual(attend.Execute(payload), "Confirmed");
            //gateway.All().Atendees;
        }
        
       
        
    }
}