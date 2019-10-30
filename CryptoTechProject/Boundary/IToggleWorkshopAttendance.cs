using CryptoTechProject.Boundary;

namespace CryptoTechProject
{
    public interface IToggleWorkshopAttendance
    {
        string Execute(ToggleWorkshopAttendanceRequest request);
    }
}