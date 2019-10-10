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

                string slackResponse =
                    "{\"blocks\": [{\"type\": \"section\",\"text\": {\"type\": \"plain_text\",\"emoji\": true,\"text\": \"Please select a workshop event for each week:\"}}," +
                    "{\"type\": \"divider\"},{\"type\": \"section\",\"text\": {\"type\": \"mrkdwn\",\"text\": \"*CRYPTOTECH*\nFriday, October 11 1:30-5:00pm\nLevel 2 - 4 O' Meara St.\n*4 guests*\"}," +
                    "\"accessory\": {\"type\": \"image\",\"image_url\": \"https://api.slack.com/img/blocks/bkb_template_images/notifications.png\",\"alt_text\": \"calendar thumbnail\"}},{\"type\": \"context\"," +
                    "\"elements\": [{\"type\": \"image\",\"image_url\": \"https://api.slack.com/img/blocks/bkb_template_images/notificationsWarningIcon.png\",\"alt_text\": \"notifications warning icon\"}," +
                    "{\"type\": \"mrkdwn\",\"text\": \"*Conflicts with Team Huddle Presentation 2:15-4:30pm*\"}]},{\"type\": \"divider\"},{\"type\": \"section\",\"text\": {\"type\": \"mrkdwn\",\"text\": \"*Workshops:*\"}}," +
                    "{\"type\": \"section\",\"text\": {\"type\": \"mrkdwn\",\"text\": \"*Friday, October 11 - 2:00-3:00pm*\n*How do you 'measure' Agile-Lean maturity in a team and why would you?*\n@zelda\"}," +
                    "\"accessory\": {\"type\": \"button\",\"text\": {\"type\": \"plain_text\",\"emoji\": true,\"text\": \"Choose\"},\"value\": \"click_me_123\"}},{\"type\": \"section\"," +
                    "\"text\": {\"type\": \"mrkdwn\",\"text\": \"*Friday, October 11 - 2:00-3:30pm*\n*Terraform: adding resources not supported by a provider (part 3) - still dangerous, you have been warned...*\n @iris\"},\"accessory\": {\"type\": \"button\"," +
                    "\"text\": {\"type\": \"plain_text\",\"emoji\": true,\"text\": \"Choose\"},\"value\": \"click_me_123\"}},{\"type\": \"section\",\"text\": {\"type\": \"mrkdwn\",\"text\": \"*Friday, October 11 - 3:00-4:00pm*\n*AWS Cloud Practitioner Certification - Practise Exam questions* \n@irene, ~@johno~\"}," +
                    "\"accessory\": {\"type\": \"button\",\"text\": {\"type\": \"plain_text\",\"emoji\": true,\"text\": \"Choose\"},\"value\": \"click_me_123\"}},{\"type\": \"section\",\"text\": {\"type\": \"mrkdwn\"," +
                    "\"text\": \"*<fakelink.ToMoreTimes.com|Show more times>*\"}},{\"type\": \"section\",\"text\": {\"type\": \"mrkdwn\",\"text\": \"Pick an item from the dropdown list\"}," +
                    "\"accessory\": {\"type\": \"static_select\",\"placeholder\": {\"type\": \"plain_text\",\"text\": \"Select an item\",\"emoji\": true},\"options\": [{\"text\": {\"type\": \"plain_text\"," +
                    "\"text\": \"Week 1\",\"emoji\": true},\"value\": \"value-0\"},{\"text\": {\"type\": \"plain_text\",\"text\": \"Week 2\",\"emoji\": true},\"value\": \"value-1\"}," +
                    "{\"text\": {\"type\": \"plain_text\",\"text\": \"Week 3\",\"emoji\": true},\"value\": \"value-2\"},{\"text\": {\"type\": \"plain_text\",\"text\": \"Week 4\",\"emoji\": true}," +
                    "\"value\": \"value-1\"}]}},{\"type\": \"divider\"},{\"type\": \"divider\"}]}";

                string the_json_to_give_to_slack = JsonConvert.SerializeObject(
                    new SlackMessage
                    {
                        Blocks = new[]
                        {
                            new SlackMessage.SectionBlock()
                            {
                                Text = new SlackMessage.SectionBlock.SectionBlockText
                                {
                                    Type = "plain_text",
                                    Emoji = true,
                                    Text = $"Workshop Name: {workshops.PresentableWorkshops[0].Name}"
                                }
                            }
                        }
                    }
                );

                byte[] responseArray = Encoding.UTF8.GetBytes(slackResponse);
                response.OutputStream.Write(responseArray, 0, responseArray.Length);

                response.KeepAlive = false;
                response.Close();
            }
        }
    }
}