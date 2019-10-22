using FluentSim;

namespace CryptoTechProject.Tests
{
    public class AirtableSimulator
    {
        public FluentSimulator simulator;
        public string maxRecords = "20";

        public AirtableSimulator() => simulator = new FluentSimulator("http://localhost:8080/");

        public void Start() => simulator.Start();

        public void Stop() => simulator.Stop();

        public void SetUp(string TABLE_ID, string AIRTABLE_API_KEY, AirtableResponse expectedResponse) =>
            simulator.Get("/v0/" + TABLE_ID + "/Marketplace")
                .WithParameter("maxRecords", maxRecords)
                .WithParameter("api_key", AIRTABLE_API_KEY)
                .WithParameter("view", "Upcoming")
                .Responds(expectedResponse);
    }
}