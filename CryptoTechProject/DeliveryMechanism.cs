using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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

                    string payload = new StreamReader(context.Request.InputStream).ReadToEnd();
                    var deserialisedPayload = DeserializeSlackButtonPayload(payload);
                    
                    BookWorkshopAttendanceRequest bookWorkshopAttendanceRequest = new BookWorkshopAttendanceRequest()
                    {
                        User = deserialisedPayload.User.Name,
                        WorkshopId = deserialisedPayload.Actions[0].Value,
                    };

                    BookWorkshopAttendance bookWorkshopAttendance = new BookWorkshopAttendance(gateway, gateway);
                    bookWorkshopAttendance.Execute(bookWorkshopAttendanceRequest);
                    
                    var jsonForSlack = SetupAndSendResponseToSlack(gateway, deserialisedPayload.User.Name, response);

                    WebClient client = new WebClient();
                    client.Headers.Add("Content-Type", "application/json");
                    client.UploadString(deserialisedPayload.ResponseURL, "POST", jsonForSlack);

                  
                }
                else
                {
                    var payload = new StreamReader(context.Request.InputStream).ReadToEnd();

                    var payloadString = HttpUtility.ParseQueryString(payload);
                    
                    string user = payloadString.Get("user_name");
                    
                    var jsonForSlack = SetupAndSendResponseToSlack(gateway, user, response);
                    
                    byte[] responseArray = Encoding.UTF8.GetBytes(jsonForSlack);
                    response.AddHeader("Content-type", "application/json");
                    response.OutputStream.Write(responseArray, 0, responseArray.Length);
                    Console.WriteLine("no payload");
                    
                }
                
                response.KeepAlive = false;
                response.Close();
            }
        }

        private static SlackButtonPayload DeserializeSlackButtonPayload(string payload)
        {
            var payloadString = HttpUtility.ParseQueryString(payload);

            Dictionary<string, string> dictionary = payloadString.Keys.Cast<string>()
                .ToDictionary(k => k, k => payloadString[k]);

            SlackButtonPayload deserialisedPayload =
                JsonConvert.DeserializeObject<SlackButtonPayload>(dictionary["payload"]);
            return deserialisedPayload;
        }

        private static string SetupAndSendResponseToSlack(AirtableGateway gateway, string deserialisedPayload,
            HttpListenerResponse response)
        {
            GetWorkshopsResponse workshops = new GetWorkshops(gateway).Execute();

            var slackMessage = ToSlackMessage(workshops, deserialisedPayload);
            string jsonForSlack = JsonConvert.SerializeObject(slackMessage);
            
            return jsonForSlack;
        }

        private static SlackMessage ToSlackMessage(GetWorkshopsResponse workshops, String User)
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
                if (workshops.PresentableWorkshops[i].Attendees.Contains(User))
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