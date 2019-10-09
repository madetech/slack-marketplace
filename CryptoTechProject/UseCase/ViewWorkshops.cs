using Newtonsoft.Json.Linq;

namespace CryptoTechProject
{
    public interface IViewWorkshopsGateway
    {
        string All();
    }
    
    public class ViewWorkshops
    {
        private IViewWorkshopsGateway gateway;
        public ViewWorkshops(IViewWorkshopsGateway viewGateway)
        {
            gateway = viewGateway;
        }
        
        public JObject Execute()
        {
            
            if (gateway.All().Length == 0)
            {
                return new JObject();
            }
            JObject json = new JObject();
            json.Add("name","Duplication");
            return json;

        }
    }
}