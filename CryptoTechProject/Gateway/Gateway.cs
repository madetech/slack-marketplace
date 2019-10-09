using System.Diagnostics.CodeAnalysis;
using CryptoTechProject.Domain;

namespace CryptoTechProject
{
    public class Gateway : IViewWorkshopsGateway
    {
        private Workshop input;
        public Gateway(Workshop workshop)
        {
            input = workshop;
        }

        public Workshop All()
        {
            return input;
        }
    }
}