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

        public void SetUpAll(AirtableResponse expectedResponse, string TABLE_ID, string AIRTABLE_API_KEY) =>
            simulator.Get("/v0/" + TABLE_ID + "/Marketplace")
                .WithParameter("maxRecords", maxRecords)
                .WithParameter("api_key", AIRTABLE_API_KEY)
                .WithParameter("view", "Upcoming")
                .Responds(expectedResponse);
        
        public void SetUpFind(string TABLE_ID, string AIRTABLE_API_KEY, AirtableResponse.WorkshopRecord expectedResponse, string workshopID) =>
            simulator.Get("/v0/" + TABLE_ID + "/Marketplace/" + workshopID)
                .WithHeader("Authorization", "Bearer " + AIRTABLE_API_KEY)
                .WithHeader("Content-Type", "application/json")
                .Responds(expectedResponse);

        public void SetUpSave(string TABLE_ID, string AIRTABLE_API_KEY) =>
            simulator.Patch("/v0/" + TABLE_ID + "/Marketplace/")
                .WithHeader("Authorization", "Bearer " + AIRTABLE_API_KEY)
                .WithHeader("Content-Type", "application/json")
                .Responds("Seaweed");
    }
}