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

                
                string the_json_to_give_to_slack = JsonConvert.SerializeObject(
                    new SlackMessage
                    {
                        Blocks = new SlackMessage.SlackMessageBlock[]
                        {
                            
                            new SlackMessage.SectionBlock()
                            {
                                Text = new SlackMessage.SectionBlock.SectionBlockText
                                {
                                    Type = "mrkdwn",
                                    Text = "*Workshops*"
                                }
                            },
                            
                            new SlackMessage.DividerBlock()
                            {
                                Type = "divider"
                            },
                            
                            new SlackMessage.SectionBlock()
                            {
                                Text = new SlackMessage.SectionBlock.SectionBlockText
                                {
                                    Type = "mrkdwn",
                                    Text = $"{workshops.PresentableWorkshops[0].Name}\n" +
                                           $"{workshops.PresentableWorkshops[0].Time.DateTime.ToString("dd/MM/yyyy hh:mm tt")}\n" +
                                           $"{workshops.PresentableWorkshops[0].Host}"
                                }
                            },
                            
                            new SlackMessage.SectionBlock()
                            {
                                Text = new SlackMessage.SectionBlock.SectionBlockText
                                {
                                    Type = "mrkdwn",
                                    Text = $"{workshops.PresentableWorkshops[1].Name}\n" +
                                           $"{workshops.PresentableWorkshops[1].Time.DateTime.ToString("dd/MM/yyyy hh:mm tt")}\n" +
                                           $"{workshops.PresentableWorkshops[1].Host}"
                                }
                            }

                        }
                    }
                );


                byte[] responseArray = Encoding.UTF8.GetBytes(the_json_to_give_to_slack);
                response.AddHeader("Content-type","application/json");
                response.OutputStream.Write(responseArray, 0, responseArray.Length);

                response.KeepAlive = false;
                response.Close();
            }
        }
    }
}