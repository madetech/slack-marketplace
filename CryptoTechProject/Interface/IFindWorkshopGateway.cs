using CryptoTechProject.Domain;

namespace CryptoTechProject
{
    public interface IFindWorkshopGateway
    {
        Workshop Find(string workshopID);
    }
}