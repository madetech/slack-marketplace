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

        public void Run(Action onStarted)
        {
            httpListener.Prefixes.Add($"http://+:{System.Environment.GetEnvironmentVariable("PORT")}/");
            httpListener.Start();
            onStarted();
            while (true)
            {
                HttpListenerContext context = httpListener.GetContext();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;
                
                AirtableGateway gateway = new AirtableGateway(System.Environment.GetEnvironmentVariable("AIRTABLE_URL"),
                    System.Environment.GetEnvironmentVariable("COPY_AIRTABLE_API_KEY"),
                    System.Environment.GetEnvironmentVariable("COPY_AIRTABLE_TABLE_ID"));

                if (request.Url.ToString().Contains("attend"))
                {
                    var payload = new StreamReader(context.Request.InputStream).ReadToEnd();

                    var payloadString = HttpUtility.ParseQueryString(payload);

                    Dictionary<string, string> dictionary = payloadString.Keys.Cast<string>()
                        .ToDictionary(k => k, k => payloadString[k]);

                    SlackButtonPayload deserialisedPayload =
                        JsonConvert.DeserializeObject<SlackButtonPayload>(dictionary["payload"]);
                    Console.WriteLine(deserialisedPayload.Actions[0].Value);

                    ToggleWorkshopAttendanceRequest toggleWorkshopAttendanceRequest = new ToggleWorkshopAttendanceRequest()
                    {
                        User = deserialisedPayload.User.Name,
                        WorkshopId = deserialisedPayload.Actions[0].Value
                    };

                    ToggleWorkshopAttendance toggleWorkshopAttendance = new ToggleWorkshopAttendance(gateway, gateway);
                    toggleWorkshopAttendance.Execute(toggleWorkshopAttendanceRequest);
                    Console.WriteLine("but did it add?");
                }
                else
                {
                    GetWorkshopsResponse workshops = new GetWorkshops(gateway).Execute();
                    var slackMessage = ToSlackMessage(workshops);
                    string jsonForSlack = JsonConvert.SerializeObject(slackMessage);
                    byte[] responseArray = Encoding.UTF8.GetBytes(jsonForSlack);
                    response.AddHeader("Content-type", "application/json");
                    response.OutputStream.Write(responseArray, 0, responseArray.Length);
                    Console.WriteLine("no payload");
                }
                
                response.KeepAlive = false;
                response.Close();
            }
        }

        private static SlackMessage ToSlackMessage(GetWorkshopsResponse workshops)
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
                            Text = "Attend"
                        },

                        Value = workshops.PresentableWorkshops[i].ID
                    }
                };
            }

            return slackMessage;
        }
    }
}