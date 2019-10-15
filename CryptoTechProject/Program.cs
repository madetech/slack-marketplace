using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Net.Http;
using CryptoTechProject.Boundary;
using dotenv.net;

namespace CryptoTechProject
{
    class Program
    {
        static void Main(string[] args)
        {
            new DeliveryMechanism().Run((() => Console.WriteLine("Server Started")));
        }
    }
}