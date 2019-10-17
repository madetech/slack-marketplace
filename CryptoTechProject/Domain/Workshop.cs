using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace CryptoTechProject.Domain
{
    public class Workshop
    {
        public DateTime time { get; set; }
        public string host { get; set; }
        public string name { get; set; }
        public string location { get; set; }
        public int duration { get; set; }
        public string type { get; set; }
    }
}