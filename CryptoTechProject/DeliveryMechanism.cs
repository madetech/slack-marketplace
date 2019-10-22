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
            //httpListener.Prefixes.Add("http://localhost:5000/");
            httpListener.Start();
            onStarted();
            while (true)
            {
                HttpListenerContext context = httpListener.GetContext();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;


                AirtableGateway gateway = new AirtableGateway(System.Environment.GetEnvironmentVariable("AIRTABLE_URL"),
                    System.Environment.GetEnvironmentVariable("AIRTABLE_API_KEY"),
                    System.Environment.GetEnvironmentVariable("AIRTABLE_TABLE_ID"));

                // AirtableGateway gateway = new AirtableGateway("https://api.airtable.com/", "Api_key",
                //  "table_id");


                if (request.Url.ToString().Contains("attend"))
                {
                    var body = new StreamReader(context.Request.InputStream).ReadToEnd();

                    var bodyString = HttpUtility.ParseQueryString(body);

                    Dictionary<string, string> dictionary = bodyString.Keys.Cast<string>()
                        .ToDictionary(k => k, k => bodyString[k]);

                    SlackButtonPayload payload =
                        JsonConvert.DeserializeObject<SlackButtonPayload>(dictionary["payload"]);
                    Console.WriteLine(payload.Actions[0].Value);
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
                               $"{workshops.PresentableWorkshops[i].Location}"
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