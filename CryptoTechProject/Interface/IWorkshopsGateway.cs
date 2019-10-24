using System.Collections.Generic;
using CryptoTechProject.Domain;

namespace CryptoTechProject
{
    public interface IWorkshopsGateway : ISaveWorkshopsGateway
    {
        List<Workshop> All();
    }
}