using System.Diagnostics.CodeAnalysis;

namespace CryptoTechProject
{
    public class Gateway : IViewWorkshopsGateway
    {
        private string input;
        public Gateway(string stringInput)
        {
            input = stringInput;
        }

        public string All()
        {
            return input;
        }
    }
}