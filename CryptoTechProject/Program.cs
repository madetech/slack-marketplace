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
            var airtableGateway = new AirtableGateway(System.Environment.GetEnvironmentVariable("AIRTABLE_URL"),
                System.Environment.GetEnvironmentVariable("AIRTABLE_API_KEY"),
                System.Environment.GetEnvironmentVariable("AIRTABLE_TABLE_ID"));
            new DeliveryMechanism(new ToggleWorkshopAttendance(airtableGateway, airtableGateway), new GetWorkshops(airtableGateway), System.Environment.GetEnvironmentVariable("PORT")).Run((() => Console.WriteLine("Server Started")));
        }
    }
}