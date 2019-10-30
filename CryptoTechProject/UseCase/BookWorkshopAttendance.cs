using System.Collections.Generic;
using System.IO.Enumeration;
using CryptoTechProject.Boundary;
using CryptoTechProject.Domain;

namespace CryptoTechProject
{
    public class BookWorkshopAttendance
    {
        private IUpdateWorkshopsGateway Saver;
        private IFindWorkshopGateway Finder;

        public BookWorkshopAttendance(IUpdateWorkshopsGateway gateway, IFindWorkshopGateway stub)
        {
            Saver = gateway;
            Finder = stub;
        }

        public string Execute(BookWorkshopAttendanceRequest request)
        {
            Workshop workshop = Finder.Find(request.WorkshopId);
            
            if (workshop.attendees.Contains(request.User))
                workshop.attendees.Remove(request.User);
            else
                workshop.attendees.Add(request.User);
            Saver.Update(workshop);
            return "Confirmed";
        }
    }
}