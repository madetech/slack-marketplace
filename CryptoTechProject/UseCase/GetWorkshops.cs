using System.Collections.Generic;
using CryptoTechProject.Boundary;
using CryptoTechProject.Domain;
using Newtonsoft.Json.Linq;

namespace CryptoTechProject
{
    public interface IGetWorkshopsGateway
    {
        List<Workshop> All();
    }
    
    public class GetWorkshops
    {
        private IGetWorkshopsGateway gateway;
        public GetWorkshops(IGetWorkshopsGateway getGateway)
        {
            gateway = getGateway;
        }
        
        public GetWorkshopsResponse Execute()
        {

            return new GetWorkshopsResponse()
            {
                PresentableWorkshops = new PresentableWorkshop[]{}
            };

        }
    }
}