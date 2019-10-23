using System.IO.Enumeration;
using CryptoTechProject.Boundary;
using CryptoTechProject.Domain;

namespace CryptoTechProject
{        
    
    public class BookWorkshopAttendance
    {
        private IGetWorkshopsGateway Gateway;
        public BookWorkshopAttendance(IGetWorkshopsGateway gateway)
        {
            Gateway = gateway;
        }

        public string Execute(BookWorkshopAttendanceRequest request)
        {
            //if (Gateway.find(payload.Workshop).Atendees.Include(payload.Name))
            
            // payload to doman payload here:
            Workshop workshop = new Workshop();
            workshop.attendees = request.User;
            workshop.id = request.Id;
            //the_workshop = Gateway.find(ID)
            
            Gateway.Save(workshop);
            return "Confirmed";
        }
    }
}