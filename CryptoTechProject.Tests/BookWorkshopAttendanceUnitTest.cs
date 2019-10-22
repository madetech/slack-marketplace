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
            BookWorkshopAttendanceRequest payload = new BookWorkshopAttendanceRequest();
            payload.User = "Maria";
            payload.Id = "Workshop Name";
            Assert.AreEqual(attend.Execute(payload), "Confirmed");
            //gateway.All().Atendees;
        }
        
       
        
    }
}