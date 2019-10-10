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
            var list = gateway.All();
            if (list.Count == 0)
                return new GetWorkshopsResponse()
                {
                    PresentableWorkshops = new PresentableWorkshop[]{}
                };
            
            return new GetWorkshopsResponse()
            {
                PresentableWorkshops = new PresentableWorkshop[]
                {
                    new PresentableWorkshop()
                    {
                        Name = list[0].name,
                        Host = list[0].host,
                        Time = list[0].time,
                        Location = list[0].location,
                        Duration = list[0].duration,
                        Type = list[0].type
                    }
                }
            };
            
        }
        
    }
}