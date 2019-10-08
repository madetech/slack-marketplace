using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Net.Http;

namespace CryptoTechProject
{
    class Program
    {
        static HttpListener _httpListener = new HttpListener();
        static void Main(string[] args)
        {
            _httpListener.Prefixes.Add($"http://+:{System.Environment.GetEnvironmentVariable("PORT")}/"); 
            _httpListener.Start(); 
             
            while(true)
            { 
                HttpListenerContext context = _httpListener.GetContext(); 

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