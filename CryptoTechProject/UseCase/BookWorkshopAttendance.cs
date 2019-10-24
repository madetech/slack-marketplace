using System.Collections.Generic;
using System.IO.Enumeration;
using CryptoTechProject.Boundary;
using CryptoTechProject.Domain;

namespace CryptoTechProject
{
    public class BookWorkshopAttendance
    {
        private ISaveWorkshopsGateway Gateway;

        public BookWorkshopAttendance(ISaveWorkshopsGateway gateway)
        {
            Gateway = gateway;
        }

        public string Execute(BookWorkshopAttendanceRequest request)
        {
            Workshop workshop = new Workshop();
            workshop.attendees = new List<string> {request.User};
            workshop.id = request.WorkshopId;

            Gateway.Save(workshop);
            return "Confirmed";
        }
    }
}