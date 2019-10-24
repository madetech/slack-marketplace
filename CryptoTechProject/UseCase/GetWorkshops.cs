using System;
using System.Linq;
using CryptoTechProject.Boundary;
using Newtonsoft.Json.Linq;

namespace CryptoTechProject
{
    public class GetWorkshops
    {
        private IWorkshopsGateway gateway;

        // Constructor below:
        public GetWorkshops(IWorkshopsGateway gateway)
        {
            this.gateway = gateway;
        }

        public GetWorkshopsResponse Execute()
        {
            var list = gateway.All();
            if (list.Count == 0)
                return new GetWorkshopsResponse()
                {
                    PresentableWorkshops = new PresentableWorkshop[] { }
                };

            var getWorkshopsResponse = new GetWorkshopsResponse()
            {
                PresentableWorkshops = new PresentableWorkshop[list.Count]
            };
            for (int i = 0; i < list.Count; i++)
            {
                getWorkshopsResponse.PresentableWorkshops[i] = new PresentableWorkshop()
                {
                    ID = list[i].id,
                    Name = list[i].name,
                    Host = list[i].host,
                    // Time = list[i].time.ToLocalTime(),
                    Time = new DateTimeOffset(list[i].time, TimeSpan.Zero).ToOffset(TimeZoneInfo
                        .FindSystemTimeZoneById("Europe/London").GetUtcOffset(list[i].time)),
                    Location = list[i].location,
                    Duration = list[i].duration,
                    Type = list[i].type
                };
            }

            return getWorkshopsResponse;
        }
    }
}