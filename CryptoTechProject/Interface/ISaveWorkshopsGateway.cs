using CryptoTechProject.Domain;

namespace CryptoTechProject
{
    public interface ISaveWorkshopsGateway
    {
        void Save(Workshop workshop);
    }
}