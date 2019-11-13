using System;
using System.IO;
using System.Net;
using System.Web;


namespace CryptoTechProject
{
    public partial class DeliveryMechanism
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
            httpListener.Start();
            onStarted();
            while (true)
            {
                HttpListenerContext context = httpListener.GetContext();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;

                if (request.Url.ToString().Contains("attend"))
                {
                    new ToggleAttendanceController().ToggleAttendance(context, _toggleWorkshopAttendance,
                        _getWorkshops);
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
    }
}