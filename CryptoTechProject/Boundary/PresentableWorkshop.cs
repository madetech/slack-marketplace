using System;
using System.Collections.Generic;

namespace CryptoTechProject.Boundary
{
    public class PresentableWorkshop
    {
        public string ID { get; set; }
        public DateTimeOffset Time { get; set; }

        public string Host { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int Duration { get; set; }
        public string Type { get; set; }
        public List<string> Attendees { get; set; }
    }
}