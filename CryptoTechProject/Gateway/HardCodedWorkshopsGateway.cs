using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CryptoTechProject.Domain;

namespace CryptoTechProject
{
    public class HardCodedWorkshopsGateway : IWorkshopsGateway
    {
        public List<Workshop> All()
        {
            DateTime time = new DateTime(2019, 10, 18, 14, 00, 0);
           // DateTimeOffset time = new DateTimeOffset(sourceDate,
            //    TimeZoneInfo.FindSystemTimeZoneById("Europe/London").GetUtcOffset(sourceDate));
           // DateTime time = sourceDate;

            DateTime time2 = new DateTime(2019, 10, 18, 15, 30, 0);
            //DateTimeOffset time2 = new DateTimeOffset(sourceDate2,
              //  TimeZoneInfo.FindSystemTimeZoneById("Europe/London").GetUtcOffset(sourceDate2));

            return new List<Workshop>()
            {
                new Workshop()
                {
                    name = "Team Performance: Team Agile-Lean maturity 'measures' in practice (at DfE and Hackney)",
                    host = "Barry",
                    time = time,
                    location = "Everest",
                    duration = 60,
                    type = "Seminar"
                },
                new Workshop()
                {
                    name = "Account Leadership - Roles & Responsibilities",
                    host = "Rory",
                    time = time2,
                    location = "Everest",
                    duration = 60,
                    type = "Workshop"
                }
            };
        }

        public void Save(Workshop workshop)
        {
            
        }
    }
}