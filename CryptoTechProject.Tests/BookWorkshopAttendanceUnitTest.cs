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
        public class SpyGateway : IUpdateWorkshopsGateway
        {
            public Workshop lastSavedWorkshop;

            public void Update(Workshop workshop)
            {
                lastSavedWorkshop = workshop;
            }
        }

        public class FindSpyStub : IFindWorkshopGateway
        {
            public Workshop existingWorkshop;
            public string lastWorkShopId;

            public Workshop Find(string workshopID)
            {
                lastWorkShopId = workshopID;
                return existingWorkshop;
            }
        }

        [Test]
        public void SaveNewAttendeeToWorkshopWithNoExistingAttendees()
        {
            FindSpyStub findSpyStub = new FindSpyStub();
            findSpyStub.existingWorkshop = new Workshop()
            {
                attendees = new List<string>()
            };
            SpyGateway spy = new SpyGateway();
            BookWorkshopAttendance bookAttendance = new BookWorkshopAttendance(spy, findSpyStub);
            BookWorkshopAttendanceRequest payload = new BookWorkshopAttendanceRequest();
            payload.User = "Seaweed";
            payload.WorkshopId = "Seaweed on holiday";
            var response = bookAttendance.Execute(payload);
            Assert.AreEqual("Seaweed", spy.lastSavedWorkshop.attendees[0]);
            Assert.AreEqual("Confirmed", response);
            Assert.AreEqual("Seaweed on holiday", findSpyStub.lastWorkShopId);
        }

        [Test]
        public void SaveNewAttendeeToWorkshopWithExistingAttendees()
        {
            FindSpyStub findSpyStub = new FindSpyStub();
            findSpyStub.existingWorkshop = new Workshop()
            {
                attendees = new List<string>()
                {
                    "Cait"
                }
            };
            SpyGateway spy = new SpyGateway();
            BookWorkshopAttendance bookAttendance = new BookWorkshopAttendance(spy, findSpyStub);
            BookWorkshopAttendanceRequest payload = new BookWorkshopAttendanceRequest();
            payload.User = "Seaweed";
            payload.WorkshopId = "16";
            bookAttendance.Execute(payload);
            Assert.AreEqual("Seaweed", spy.lastSavedWorkshop.attendees[1]);
            Assert.AreEqual("Cait", spy.lastSavedWorkshop.attendees[0]);
            Assert.AreEqual("16", findSpyStub.lastWorkShopId);
        }
        
        [Test]
        public void RemovesAttendeeIfAlreadyInWorkshopAttendees()
        {
            FindSpyStub findSpyStub = new FindSpyStub();
            findSpyStub.existingWorkshop = new Workshop()
            {
                attendees = new List<string>()
                {
                    "Cait", "Maria"
                }
            };
            SpyGateway spy = new SpyGateway();
            BookWorkshopAttendance bookAttendance = new BookWorkshopAttendance(spy, findSpyStub);
            BookWorkshopAttendanceRequest payload = new BookWorkshopAttendanceRequest();
            payload.User = "Maria";
            payload.WorkshopId = "16";
            bookAttendance.Execute(payload);
            Assert.IsFalse(spy.lastSavedWorkshop.attendees.Contains("Maria"));
            Assert.AreEqual("16", findSpyStub.lastWorkShopId);
        }
    }
}