using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CryptoTechProject.Domain;

namespace CryptoTechProject
{
    public class HardCodedWorkshopsGateway : IGetWorkshopsGateway
    {
        public List<Workshop> All()
        {
            DateTime sourceDate = new DateTime(2008, 5, 1, 8, 30, 0);
            DateTimeOffset time = new DateTimeOffset(sourceDate, 
                TimeZoneInfo.FindSystemTimeZoneById("Europe/London").GetUtcOffset(sourceDate));
            
            DateTime sourceDate2 = new DateTime(2019, 10, 18, 15, 30, 0);
            DateTimeOffset time2 = new DateTimeOffset(sourceDate2, 
                TimeZoneInfo.FindSystemTimeZoneById("Europe/London").GetUtcOffset(sourceDate2)); 
            
            return new List<Workshop>()
            {
                new Workshop()
                {
                    name = "Coding Black Females - Code Dojo",
                    host = "Made Tech",
                    time = time,
                    location = "Made Tech O'Meara",
                    duration = 180,
                    type = "Code Dojo"
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
    }
}