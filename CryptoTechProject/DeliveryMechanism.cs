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
        public DeliveryMechanism(IToggleWorkshopAttendance toggleWorkshopAttendance, IGetWorkshops getWorkshops,
            string port)
        {
            _toggleWorkshopAttendance = toggleWorkshopAttendance;
            _getWorkshops = getWorkshops;
            _port = port;
        }

        public void Run(Action onStarted)
        {
            httpListener.Prefixes.Add($"http://+:{_port}/");
           // httpListener.Prefixes.Add("http://*:443/");
            httpListener.Start();
            onStarted();
            while (true)
            {
                HttpListenerContext context = httpListener.GetContext();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;

                if (request.Url.ToString().Contains("attend"))
                {
                    ToggleAttendance(context);
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
            public void GetWorkshops(HttpListenerResponse response, IGetWorkshops getWorkshops, string user)
            {
                GetWorkshopsResponse workshops = getWorkshops.Execute();
                var slackMessage = ToSlackMessage(workshops, user);
                string jsonForSlack = JsonConvert.SerializeObject(slackMessage);
                byte[] responseArray = Encoding.UTF8.GetBytes(jsonForSlack);
                response.AddHeader("Content-type", "application/json");
                response.OutputStream.Write(responseArray, 0, responseArray.Length);
            }
        }


        private void ToggleAttendance(HttpListenerContext context)
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

            _toggleWorkshopAttendance.Execute(toggleWorkshopAttendanceRequest);

            GetWorkshopsResponse workshops = _getWorkshops.Execute();
            var slackMessage = ToSlackMessage(workshops, toggleWorkshopAttendanceRequest.User);
            string jsonForSlack = JsonConvert.SerializeObject(slackMessage);


            WebClient webClient = new WebClient();
            webClient.Headers.Add("Content-type", "application/json");
            webClient.UploadString(response_url, "POST", jsonForSlack);
        }

        private static SlackMessage ToSlackMessage(GetWorkshopsResponse workshops, string user)
        {
            var sessions = workshops.PresentableWorkshops;
            
            SlackMessage slackMessage = new SlackMessage
            {
                Blocks = new SlackMessage.SlackMessageBlock[sessions.Length + 2]
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

            for (int i = 0; i < sessions.Length; i++)
            {
                DateTimeOffset sessionEndTime =
                    sessions[i].Time.AddMinutes(sessions[i].Duration);
                
                string attendanceStatus = "Attend";
                if (sessions[i].Attendees.Contains(user))
                {
                    attendanceStatus = "Unattend";
                }
                

                if (sessions[i].Type == "Showcase")
                {
                    string showcaseText = $"*{sessions[i].Name}*\n" +
                                          $"{sessions[i].Time.ToString("dd/MM/yyyy HH:mm")} - {sessionEndTime.ToString("HH:mm")}\n" +
                                          $"{sessions[i].Host}\n";

                    if (i < sessions.Length - 1)
                        if (sessions[i].Time.Day !=
                            sessions[i + 1].Time.Day)
                            showcaseText = showcaseText +
                                           "---------------------------------------------------------------------------------------------------------\n";
                    slackMessage.Blocks[i + 2] = new SlackMessage.ShowcaseSectionBlock
                    {
                        Text = new SlackMessage.SectionBlockText
                        {
                            Type = "mrkdwn",
                            Text = showcaseText
                        }
                    };
                }
                else
                {
                    

                    slackMessage.Blocks[i + 2] = new SlackMessage.SectionBlock
                    {
                        Text = new SlackMessage.SectionBlockText
                        {
                            Type = "mrkdwn",
                            Text = $"*{sessions[i].Name}*\n" +
                                   $"{sessions[i].Time.ToString("dd/MM/yyyy HH:mm")} - {sessionEndTime.ToString("HH:mm")}\n" +
                                   $"{sessions[i].Host}\n" +
                                   $"Current number of attendees: {sessions[i].Attendees.Count}"
                        },
                        Accessory = new SlackMessage.SectionBlock.AccessoryBlock
                        {
                            Text = new SlackMessage.SectionBlockText
                            {
                                Type = "plain_text",
                                Text = attendanceStatus
                            },
                            Value = sessions[i].ID
                        }
                    };
                }
            }

            return slackMessage;
        }
    }
}