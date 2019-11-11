using System;
using System.Collections.Generic;
using CryptoTechProject.Domain;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace CryptoTechProject.Tests
{
    public class ViewWorkshopsTest
    {
        [TestFixture]
        public class WorkshopsUnitTest : IWorkshopsGateway
        {
            private List<Workshop> _workshops;

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

            [Test]
            public void ReturnsWorkshopsWithADate()
            {
                _workshops.Add(item: new Workshop()
                    {
                        name = "Something",
                        host = "somethingelse"
                    }
                );

                var response = new GetWorkshops(this).Execute();

                response.PresentableWorkshops.Should().BeNull();
            }

            public List<Workshop> All()
            {
                return _workshops;
            }
            

            public void Update(Workshop workshop)
            {
            }
        }
    }
}
