using System;
using System.Net;
using System.Text;
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
                HttpListenerResponse response = context.Response;

                //HardCodedWorkshopsGateway gateway = new HardCodedWorkshopsGateway();
                AirtableGateway gateway = new AirtableGateway(System.Environment.GetEnvironmentVariable("AIRTABLE_URL"),
                    System.Environment.GetEnvironmentVariable("AIRTABLE_API_KEY"),
                    System.Environment.GetEnvironmentVariable("AIRTABLE_TABLE_ID"));
                
                //AirtableGateway gateway = new AirtableGateway("https://api.airtable.com/", INSERT_API_KEY, INSERT_TABLE_ID);
                    
                GetWorkshopsResponse workshops = new GetWorkshops(gateway).Execute();

                var slackMessage = ToSlackMessage(workshops);


                string jsonForSlack = JsonConvert.SerializeObject(slackMessage);
                byte[] responseArray = Encoding.UTF8.GetBytes(jsonForSlack);
                response.AddHeader("Content-type", "application/json");
                response.OutputStream.Write(responseArray, 0, responseArray.Length);

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

            slackMessage.Blocks[0] = new SlackMessage.SectionBlock
            {
                Text = new SlackMessage.SectionBlock.SectionBlockText
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
                    Text = new SlackMessage.SectionBlock.SectionBlockText
                    {
                        Type = "mrkdwn",
                        Text = $"{workshops.PresentableWorkshops[i].Name}\n" +
                               $"{workshops.PresentableWorkshops[i].Time.ToString("dd/MM/yyyy hh:mm tt")}\n" +
                               $"{workshops.PresentableWorkshops[i].Host}\n"+
                               $"{workshops.PresentableWorkshops[i].Location}"
                               
                    }
                };
            }

            return slackMessage;
        }
    }
}