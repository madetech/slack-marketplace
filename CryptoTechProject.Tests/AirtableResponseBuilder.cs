using System;
using System.Collections.Generic;

namespace CryptoTechProject.Tests
{
    public class AirtableResponseBuilder
    {
        private List<AirtableResponse.WorkshopRecord> _records = new List<AirtableResponse.WorkshopRecord>();
        private AirtableResponse.Fields _currentFields;

        public AirtableResponseBuilder AddRecord(string id, DateTime createdTime)
        {
            _currentFields = new AirtableResponse.Fields();
            _records.Add(
                new AirtableResponse.WorkshopRecord()
                {
                    ID = id,
                    CreatedTime = createdTime,
                    Fields = _currentFields
                }
            );

            return this;
        }

        public AirtableResponseBuilder WithName(string name)
        {
            _currentFields.Name = name;
            return this;
        }

        public AirtableResponseBuilder WithHost(string host)
        {
            _currentFields.Host = host;
            return this;
        }

        public AirtableResponseBuilder WithCategories(params string[] categories)
        {
            _currentFields.Categories = categories;
            return this;
        }

        public AirtableResponseBuilder WithTime(int year, int month, int day, int hours, int minutes, int seconds)
        {
            _currentFields.Time = new DateTimeOffset(
                year,
                month,
                day,
                hours,
                minutes,
                seconds,
                TimeSpan.Zero
            );
            return this;
        }

        public AirtableResponseBuilder WithDurationInSeconds(int seconds)
        {
            _currentFields.Duration = seconds;
            return this;
        }

        public AirtableResponseBuilder WithLocation(string location)
        {
            _currentFields.Location = location;
            return this;
        }

        public AirtableResponseBuilder WithSessionType(string sessionType)
        {
            _currentFields.SessionType = sessionType;
            return this;
        }

        public AirtableResponse Build() =>
            new AirtableResponse
            {
                Records = _records.ToArray()
            };
    }
}