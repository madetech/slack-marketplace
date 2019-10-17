using System;
using System.Collections.Generic;
using System.Linq;
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
        // Constructor below:
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

            var getWorkshopsResponse = new GetWorkshopsResponse()
            {
                PresentableWorkshops = new PresentableWorkshop[list.Count]
            };
                for (int i = 0; i < list.Count; i++)
                {
                    getWorkshopsResponse.PresentableWorkshops[i] = new PresentableWorkshop()
                    {
                        Name = list[i].name,
                        Host = list[i].host,
                        Time = list[i].time.ToLocalTime(),
                        Location = list[i].location,
                        Duration = list[i].duration,
                        Type = list[i].type
                    };
                }
                return getWorkshopsResponse;
        }
    }
}