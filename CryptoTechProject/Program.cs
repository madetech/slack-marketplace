using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Net.Http;
using dotenv.net;

namespace CryptoTechProject
{
    class Program
    {
        static HttpListener httpListener = new HttpListener();
        static void Main(string[] args)
        {
            httpListener.Prefixes.Add($"http://+:{System.Environment.GetEnvironmentVariable("PORT")}/"); 
            httpListener.Start(); 
             
            while(true)
            { 
                HttpListenerContext context = httpListener.GetContext(); 

                HttpListenerResponse response = context.Response;
                        
                string hello = "I'm WORKING !!!'";
                byte[] responseArray = Encoding.UTF8.GetBytes(hello); 
                response.OutputStream.Write(responseArray, 0, responseArray.Length); 
                        
                response.KeepAlive = false; 
                response.Close(); 
            }
           
        }

    }
}