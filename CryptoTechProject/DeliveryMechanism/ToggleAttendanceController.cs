using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using CryptoTechProject.Boundary;
using Newtonsoft.Json;

namespace CryptoTechProject
{
    public partial class DeliveryMechanism
    {
        private class ToggleAttendanceController
        {
            public void ToggleAttendance(HttpListenerContext context,
                IToggleWorkshopAttendance toggleWorkshopAttendance, IGetWorkshops getWorkshops)
            {
                var payload = new StreamReader(context.Request.InputStream).ReadToEnd();

                var firstString = HttpUtility.UrlDecode(payload);
                var payloadString = HttpUtility.ParseQueryString(firstString);

                Dictionary<string, string> dictionary = payloadString
                    .Keys
                    .Cast<string>()
                    .ToDictionary(k => k, k => payloadString[k]);


                SlackButtonPayload deserialisedPayload =
                    JsonConvert.DeserializeObject<SlackButtonPayload>(dictionary["payload"]);

                ToggleWorkshopAttendanceRequest toggleWorkshopAttendanceRequest = new ToggleWorkshopAttendanceRequest()
                {
                    User = deserialisedPayload.User.Name,
                    WorkshopId = deserialisedPayload.Actions[0].Value
                };

                string response_url = deserialisedPayload.ResponseURL;

                toggleWorkshopAttendance.Execute(toggleWorkshopAttendanceRequest);

                GetWorkshopsResponse workshops = getWorkshops.Execute();
                SlackMessageGenerator slackMessageGenerator = new SlackMessageGenerator();
                var slackMessage = slackMessageGenerator.ToSlackMessage(workshops, toggleWorkshopAttendanceRequest.User);
                string jsonForSlack = JsonConvert.SerializeObject(slackMessage);


                WebClient webClient = new WebClient();
                webClient.Headers.Add("Content-type", "application/json");
                webClient.UploadString(response_url, "POST", jsonForSlack);
            }
        }
    }
}