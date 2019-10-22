using System.IO.Enumeration;
using CryptoTechProject.Boundary;

namespace CryptoTechProject
{        
    
    public class BookWorkshopAttendance
    {
        private IGetWorkshopsGateway Gateway;
        public BookWorkshopAttendance(IGetWorkshopsGateway gateway)
        {
            Gateway = gateway;
        }

        public string Execute(FunctionalPayload payload)
        {

            //if (Gateway.find(payload.Workshop).Atendees.Include(payload.Name))
            
            //Gateway.save(payload);
            return "Confirmed";
        }
    }
}