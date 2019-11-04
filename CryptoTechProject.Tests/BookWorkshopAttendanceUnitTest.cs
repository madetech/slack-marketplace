using System.Collections.Generic;
using System.Net;
using CryptoTechProject.Boundary;
using CryptoTechProject.Domain;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CryptoTechProject.Tests
{
    [TestFixture]
    public class BookWorkshopAttendanceUnitTest
    {
        private SpyGateway _spy;

        public BookWorkshopAttendanceUnitTest()
        {
            _spy = new SpyGateway();
        }

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
            ToggleWorkshopAttendance toggleAttendance = new ToggleWorkshopAttendance(_spy, findSpyStub);
            ToggleWorkshopAttendanceRequest payload = new ToggleWorkshopAttendanceRequest();
            payload.User = "Bogdan";
            payload.WorkshopId = "idNum3029";
            
            var response = toggleAttendance.Execute(payload);

            _spy.lastSavedWorkshop.attendees[0].Should().Be("Bogdan");
            findSpyStub.lastWorkShopId.Should().Be("idNum3029");
            response.Should().Be("Confirmed");
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
            ToggleWorkshopAttendance toggleAttendance = new ToggleWorkshopAttendance(_spy, findSpyStub);
            ToggleWorkshopAttendanceRequest payload = new ToggleWorkshopAttendanceRequest();
            payload.User = "Bogdan";
            payload.WorkshopId = "id16";
            toggleAttendance.Execute(payload);

            var workshopAttendees = _spy.lastSavedWorkshop.attendees;
            
            workshopAttendees[0].Should().Be("Cait");
            workshopAttendees[1].Should().Be("Bogdan");
            findSpyStub.lastWorkShopId.Should().Be("id16");
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
            ToggleWorkshopAttendance toggleAttendance = new ToggleWorkshopAttendance(_spy, findSpyStub);
            ToggleWorkshopAttendanceRequest payload = new ToggleWorkshopAttendanceRequest();
            payload.User = "Maria";
            payload.WorkshopId = "16";
            toggleAttendance.Execute(payload);

            _spy.lastSavedWorkshop.attendees.Should().NotContain("Maria");
            findSpyStub.lastWorkShopId.Should().Be("16");
        }
    }
}