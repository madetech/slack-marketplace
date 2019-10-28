using System.Collections.Generic;
using CryptoTechProject.Domain;

namespace CryptoTechProject
{
    public interface IWorkshopsGateway : IUpdateWorkshopsGateway
    {
        List<Workshop> All();
    }
}