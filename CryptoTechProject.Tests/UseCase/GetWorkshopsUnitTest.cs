using System;
using System.Collections.Generic;
using CryptoTechProject.Domain;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace CryptoTechProject.Tests
{
    public class GetWorkshopsTest
    {
        [TestFixture]
        public class WorkshopsUnitTest : IWorkshopsGateway
        {
            private List<Workshop> _workshops;
            
            public List<Workshop> All()
            {
                return _workshops;
            }
            

            public void Update(Workshop workshop)
            {
            }

            [SetUp]
            public void Setup()
            {
                _workshops = new List<Workshop>();
            }

            [Test]
            public void CanGetNoWorkshop()
            {
                var response = new GetWorkshops(this).Execute();
                
                response.PresentableWorkshops.Should().BeEmpty();
            }

            [Test]
            public void CanGetAWorkshop()
            {
                _workshops.Add(new Workshop()
                {
                    time = DateTime.Now
                });
                var response = new GetWorkshops(this).Execute();

                response.PresentableWorkshops.Should().NotBeEmpty();
            }
        }
    }
}
