using System.Collections.Generic;
using System.Net;
using CryptoTechProject.Boundary;
using CryptoTechProject.Domain;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CryptoTechProject.Tests
{
    [TestFixture]
    public class BookWorkshopAttendanceUnitTest
    {
        [Test]
        public void ConfirmsUserAttendance()
        {
            HardCodedWorkshopsGateway gateway = new HardCodedWorkshopsGateway();
            BookWorkshopAttendance attend = new BookWorkshopAttendance(gateway);
            BookWorkshopAttendanceRequest payload = new BookWorkshopAttendanceRequest();
            payload.User = "Maria";
            payload.WorkshopId = "Workshop Name";
            Assert.AreEqual(attend.Execute(payload), "Confirmed");
        }

        public class SpyGateway : ISaveWorkshopsGateway
        {
            public Workshop lastSavedWorkshop = new Workshop()
            {
                id = "1234",
                attendees = new List<string>()
                {
                    "Bing"
                }
            };

            public void Save(Workshop workshop)
            {
                lastSavedWorkshop = workshop;
            }
        }

        [Test]
        public void SpecificObjectIsCreatedProperly()
        {
            SpyGateway spy = new SpyGateway();
            BookWorkshopAttendance bookAttendance = new BookWorkshopAttendance(spy);
            BookWorkshopAttendanceRequest payload = new BookWorkshopAttendanceRequest();
            payload.User = "Seaweed";
            payload.WorkshopId = "Seaweed on holiday";
            bookAttendance.Execute(payload);
            Assert.AreEqual("Seaweed", spy.lastSavedWorkshop.attendees[0]);
            Assert.AreEqual("Seaweed on holiday", spy.lastSavedWorkshop.id);
        }

        [Test]
        public void SaveNewAttendeeToWorkshopWithExistingAttendees()
        {
            SpyGateway spy = new SpyGateway();
            BookWorkshopAttendance bookAttendance = new BookWorkshopAttendance(spy);
            BookWorkshopAttendanceRequest payload = new BookWorkshopAttendanceRequest();
            payload.User = "Seaweed";
            payload.WorkshopId = "Seaweed on holiday";
            bookAttendance.Execute(payload);
            Assert.AreEqual("Seaweed", spy.lastSavedWorkshop.attendees[0]);
            Assert.AreEqual("Seaweed on holiday", spy.lastSavedWorkshop.id);
            
        }
        
    }
}