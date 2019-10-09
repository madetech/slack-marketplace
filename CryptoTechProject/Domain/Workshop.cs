using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace CryptoTechProject.Domain
{
    public class Workshop
    {
        public string time { get; }
        public string host { get; }
        public string name { get; }
        public string location { get; }
        public string duration { get; }
        public string type { get; }

        public Workshop(Dictionary<string, string> details)
        {
            time = details["time"];
            host = details["host"];
            name = details["name"];
            location = details["location"];
            duration = details["duration"];
            type = details["type"];
        }

        public Workshop()
        {
            time = "";
            host = "";
            name = "";
            location = "";
            duration = "";
            type = "";
        }

    }
}