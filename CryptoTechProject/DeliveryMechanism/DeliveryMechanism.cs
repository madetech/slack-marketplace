using System;
using System.IO;
using System.Net;
using System.Web;


namespace CryptoTechProject
{
    public partial class DeliveryMechanism
    {
        public HttpListener httpListener;

        private IToggleWorkshopAttendance _toggleWorkshopAttendance;
        private IGetWorkshops _getWorkshops;
        private readonly string _port;

        public DeliveryMechanism(IToggleWorkshopAttendance toggleWorkshopAttendance, IGetWorkshops getWorkshops,
            string port)
        {
            httpListener = new HttpListener();
            _toggleWorkshopAttendance = toggleWorkshopAttendance;
            _getWorkshops = getWorkshops;
            _port = port;
        }

        public void Run(Action onStarted)
        {
            httpListener.Prefixes.Add($"http://+:{_port}/");
            httpListener.Start();
            onStarted();
            httpListener.BeginGetContext(ProcessRequest, httpListener);

            void ProcessRequest(IAsyncResult result)
            {
                try
                {
                    //If we are not listening this line throws a ObjectDisposedException.
                    HttpListenerContext context = ((HttpListener) result.AsyncState).EndGetContext(result);

                    /// aah
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
                    // end of aaah

                    context.Response.Close();
                    httpListener.BeginGetContext(ProcessRequest, httpListener);
                }
                catch (ObjectDisposedException)
                {
                    //Intentionally not doing anything with the exception.
                }
            }
            
            
           /* onStarted();
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
            }*/
        }
    }
}