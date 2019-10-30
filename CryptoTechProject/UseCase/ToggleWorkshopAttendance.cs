using CryptoTechProject.Boundary;
using CryptoTechProject.Domain;

namespace CryptoTechProject
{
    public class ToggleWorkshopAttendance : IToggleWorkshopAttendance
    {
        private IUpdateWorkshopsGateway Updater;
        private IFindWorkshopGateway Finder;

        public ToggleWorkshopAttendance(IUpdateWorkshopsGateway gateway, IFindWorkshopGateway stub)
        {
            Updater = gateway;
            Finder = stub;
        }

        public string Execute(ToggleWorkshopAttendanceRequest request)
        {
            Workshop workshop = Finder.Find(request.WorkshopId);
            if (workshop.attendees.Contains(request.User))
                workshop.attendees.Remove(request.User);
            else
                workshop.attendees.Add(request.User);
            Updater.Update(workshop);
            return "Confirmed";
        }
    }
}