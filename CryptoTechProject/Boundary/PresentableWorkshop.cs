using System;

namespace CryptoTechProject.Boundary
{
    public class PresentableWorkshop
    {
        public DateTime Time { get; set; }

        public string Host { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int Duration { get; set; }
        public string Type { get; set; }
    }
}