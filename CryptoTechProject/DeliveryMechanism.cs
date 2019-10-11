using System.Net;
using System.Text;
using CryptoTechProject.Boundary;
using Newtonsoft.Json;

namespace CryptoTechProject
{
    public class DeliveryMechanism
    {
        HttpListener httpListener = new HttpListener();

        public void Run()
        {
            httpListener.Prefixes.Add($"http://+:{System.Environment.GetEnvironmentVariable("PORT")}/");
            httpListener.Start();

            while (true)
            {
                HttpListenerContext context = httpListener.GetContext();

                HttpListenerResponse response = context.Response;

                HardCodedWorkshopsGateway gateway = new HardCodedWorkshopsGateway();
                GetWorkshopsResponse workshops = new GetWorkshops(gateway).Execute();


                SlackMessage slackMessage = new SlackMessage
                {
                    Blocks = new SlackMessage.SlackMessageBlock[workshops.PresentableWorkshops.Length + 2]
                };

                slackMessage.Blocks[0] = new SlackMessage.SectionBlock()
                {
                    Text = new SlackMessage.SectionBlock.SectionBlockText
                    {
                        Type = "mrkdwn",
                        Text = "*Workshops*"
                    }
                };

                slackMessage.Blocks[1] = new SlackMessage.DividerBlock()
                {
                    Type = "divider"
                };

                for (int i = 0; i < workshops.PresentableWorkshops.Length; i++)
                {
                    slackMessage.Blocks[i + 2] = new SlackMessage.SectionBlock()
                    {
                        Text = new SlackMessage.SectionBlock.SectionBlockText
                        {
                            Type = "mrkdwn",
                            Text = $"{workshops.PresentableWorkshops[i].Name}\n" +
                                   $"{workshops.PresentableWorkshops[i].Time.DateTime.ToString("dd/MM/yyyy hh:mm tt")}\n" +
                                   $"{workshops.PresentableWorkshops[i].Host}"
                        }
                    };
                }


                string the_json_to_give_to_slack = JsonConvert.SerializeObject(slackMessage);
                byte[] responseArray = Encoding.UTF8.GetBytes(the_json_to_give_to_slack);
                response.AddHeader("Content-type", "application/json");
                response.OutputStream.Write(responseArray, 0, responseArray.Length);

                response.KeepAlive = false;
                response.Close();
            }
        }
    }
}