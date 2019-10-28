using System.Collections.Generic;
using System.IO.Enumeration;
using CryptoTechProject.Boundary;
using CryptoTechProject.Domain;

namespace CryptoTechProject
{
    public class BookWorkshopAttendance
    {
        private ISaveWorkshopsGateway Saver;
        private IFindWorkshopGateway Finder;

        public BookWorkshopAttendance(ISaveWorkshopsGateway gateway, IFindWorkshopGateway stub)
        {
            Saver = gateway;
            Finder = stub;
        }

        public string Execute(BookWorkshopAttendanceRequest request)
        {
            Workshop workshop = Finder.Find(request.WorkshopId);
            workshop.attendees.Add(request.User);
            Saver.Save(workshop);
            return "Confirmed";
        }
    }
}