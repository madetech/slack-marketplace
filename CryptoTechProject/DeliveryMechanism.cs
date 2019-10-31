using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using CryptoTechProject.Boundary;
using Newtonsoft.Json;


namespace CryptoTechProject
{
    public class DeliveryMechanism
    {
        HttpListener httpListener = new HttpListener();
        
        private IToggleWorkshopAttendance _toggleWorkshopAttendance;
        private IGetWorkshops _getWorkshops;
        private readonly string _port;

        public DeliveryMechanism(IToggleWorkshopAttendance toggleWorkshopAttendance, IGetWorkshops getWorkshops, string port)
        {
            _toggleWorkshopAttendance = toggleWorkshopAttendance;
            _getWorkshops = getWorkshops;
            _port = port;
        }

        public void Run(Action onStarted)
        {
            httpListener.Prefixes.Add($"http://+:{_port}/");
            httpListener.Start();
            onStarted();
            while (true)
            {
                HttpListenerContext context = httpListener.GetContext();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;

                if (request.Url.ToString().Contains("attend"))
                {
                    SlackButtonPayload deserialisedPayload = ToggleAttendance(context);
                    string user = deserialisedPayload.User.Name;
                    var jsonForSlack = new GetWorkshopController().GetWorkshops(response, _getWorkshops, user);
                    
                    WebClient client = new WebClient();
                    client.Headers.Add("Content-Type", "application/json");
                    client.UploadString(deserialisedPayload.ResponseURL, "POST", jsonForSlack);
                }
                else
                {
                    var payload = new StreamReader(context.Request.InputStream).ReadToEnd();

                    var payloadString = HttpUtility.ParseQueryString(payload);
                    
                    string user = payloadString.Get("user_name");
                    new GetWorkshopController().GetWorkshops(response, _getWorkshops, user);
                }
                
                response.KeepAlive = false;
                response.Close();
            }
        }

        class GetWorkshopController
        {
            public string GetWorkshops(HttpListenerResponse response, IGetWorkshops getWorkshops, string user)
            {
                GetWorkshopsResponse workshops = getWorkshops.Execute();
                var slackMessage = ToSlackMessage(workshops, user);
                string jsonForSlack = JsonConvert.SerializeObject(slackMessage);
                byte[] responseArray = Encoding.UTF8.GetBytes(jsonForSlack);
                response.AddHeader("Content-type", "application/json");
                response.OutputStream.Write(responseArray, 0, responseArray.Length);
                return jsonForSlack;
            }
        }
        

        private SlackButtonPayload ToggleAttendance(HttpListenerContext context)
        {
            var payload = new StreamReader(context.Request.InputStream).ReadToEnd();

            var payloadString = HttpUtility.ParseQueryString(payload);

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

            _toggleWorkshopAttendance.Execute(toggleWorkshopAttendanceRequest);
            return deserialisedPayload;
        }

        private static SlackMessage ToSlackMessage(GetWorkshopsResponse workshops, string user)
        {
            SlackMessage slackMessage = new SlackMessage
            {
                Blocks = new SlackMessage.SlackMessageBlock[workshops.PresentableWorkshops.Length + 2]
            };

            slackMessage.Blocks[0] = new SlackMessage.TitleSectionBlock
            {
                Text = new SlackMessage.SectionBlockText
                {
                    Type = "mrkdwn",
                    Text = "*Workshops*"
                }
            };

            slackMessage.Blocks[1] = new SlackMessage.DividerBlock
            {
                Type = "divider"
            };

            for (int i = 0; i < workshops.PresentableWorkshops.Length; i++)
            {
                string buttonValue;
                if (workshops.PresentableWorkshops[i].Attendees.Contains(user))
                    buttonValue = "Unattend";
                else
                    buttonValue = "Attend";
                slackMessage.Blocks[i + 2] = new SlackMessage.SectionBlock
                {
                    Text = new SlackMessage.SectionBlockText
                    {
                        Type = "mrkdwn",
                        Text = $"*{workshops.PresentableWorkshops[i].Name}*\n" +
                               $"{workshops.PresentableWorkshops[i].Time.ToString("dd/MM/yyyy hh:mm tt")}\n" +
                               $"{workshops.PresentableWorkshops[i].Host}\n" +
                               $"Current number of attendees: {workshops.PresentableWorkshops[i].Attendees.Count}"
                    },
                    Accessory = new SlackMessage.SectionBlock.AccessoryBlock
                    {
                        Text = new SlackMessage.SectionBlockText
                        {
                            Type = "plain_text",
                            Text = buttonValue
                        },

                        Value = workshops.PresentableWorkshops[i].ID
                    }
                };
            }

            return slackMessage;
        }
    }
}