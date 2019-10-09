using CryptoTechProject.Domain;
using Newtonsoft.Json.Linq;

namespace CryptoTechProject
{
    public interface IViewWorkshopsGateway
    {
        Workshop All();
    }
    
    public class ViewWorkshops
    {
        private IViewWorkshopsGateway gateway;
        public ViewWorkshops(IViewWorkshopsGateway viewGateway)
        {
            gateway = viewGateway;
        }
        
        public Workshop Execute()
        {

            return gateway.All();

        }
    }
}