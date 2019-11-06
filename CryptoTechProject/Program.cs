using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Net.Http;
using CryptoTechProject.Boundary;
using dotenv.net;
using Sentry;

namespace CryptoTechProject
{
    class Program
    {
        static void Main(string[] args)

        {
            using (SentrySdk.Init("https://c5002188520c4a20ace4b35e6d826e0e@sentry.io/1806644"))
            {
                var airtableGateway = new AirtableGateway(System.Environment.GetEnvironmentVariable("AIRTABLE_URL"),
                    System.Environment.GetEnvironmentVariable("AIRTABLE_API_KEY"),
                    System.Environment.GetEnvironmentVariable("AIRTABLE_TABLE_ID"));
                new DeliveryMechanism(new ToggleWorkshopAttendance(airtableGateway, airtableGateway),
                        new GetWorkshops(airtableGateway), System.Environment.GetEnvironmentVariable("PORT"))
                    .Run((() => Console.WriteLine("Server Started")));
            }
        }
    }
}