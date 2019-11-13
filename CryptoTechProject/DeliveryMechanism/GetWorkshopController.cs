using System.Net;
using System.Text;
using CryptoTechProject.Boundary;
using Newtonsoft.Json;

namespace CryptoTechProject
{

        public class GetWorkshopController
        {
            public void GetWorkshops(HttpListenerResponse response, IGetWorkshops getWorkshops, string user)
            {
                GetWorkshopsResponse workshops = getWorkshops.Execute();
                SlackMessageGenerator slackMessageGenerator = new SlackMessageGenerator();
                var slackMessage = slackMessageGenerator.ToSlackMessage(workshops, user);
                string jsonForSlack = JsonConvert.SerializeObject(slackMessage);
                byte[] responseArray = Encoding.UTF8.GetBytes(jsonForSlack);
                response.AddHeader("Content-type", "application/json");
                response.OutputStream.Write(responseArray, 0, responseArray.Length);
            }
        }
    
}