using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using my_books.Controllers;
using my_books.Data;
using my_books.Data.Models;
using my_books.Data.Services;
using my_books.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my_books_tests
{
    public class PublishersControllerTests
    {
        private static DbContextOptions<AppDbContext> dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "BookDbControllerTest")
            .Options;

        AppDbContext context;
        PublishersService publishersService;
        PublishersController publishersController;

        [OneTimeSetUp]
        public void Setup()
        {
            context = new AppDbContext(dbContextOptions);
            context.Database.EnsureCreated();

            SeedDatabase();

            publishersService = new PublishersService(context);
            publishersController = new PublishersController(publishersService, new NullLogger<PublishersController>());
        }

        [Test, Order(1)]
        public void HTTPGET_GetAllPublishers_WithSortBySearchStringPageNo_ReturnsOk_Test()
        {
            IActionResult actionResult = publishersController.GetAllPublishers("name_desc", "Publisher", 1);
            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());
            var actionResultData = (actionResult as OkObjectResult).Value as List<Publisher>;
            Assert.That(actionResultData.First().Name, Is.EqualTo("Publisher 6"));
            Assert.That(actionResultData.First().Id, Is.EqualTo(6));
            Assert.That(actionResultData.Count, Is.EqualTo(5));

            
            IActionResult actionResultSecond = publishersController.GetAllPublishers("name_desc", "Publisher", 2);
            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());
            var actionResultDataSecond = (actionResultSecond as OkObjectResult).Value as List<Publisher>;
            Assert.That(actionResultDataSecond.First().Name, Is.EqualTo("Publisher 1").IgnoreCase);
            Assert.That(actionResultDataSecond.First().Id, Is.EqualTo(1));
            Assert.That(actionResultDataSecond.Count, Is.EqualTo(1));
        }

        [Test, Order(2)]
        public void HTTPGET_GetPublisherById_ReturnsOk_Test()
        {
            int publisherId = 1;

            IActionResult actionResult = publishersController.GetPublisherById(publisherId);

            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());
            var actionResultData = (actionResult as OkObjectResult).Value as Publisher;
            Assert.That(actionResultData.Id, Is.EqualTo(1));
            Assert.That(actionResultData.Name, Is.EqualTo("Publisher 1"));
        }

        [Test, Order(3)]
        public void HTTPGET_GetPublisherById_ReturnsNotFound_Test()
        {
            int publisherId = 99;

            IActionResult actionResult = publishersController.GetPublisherById(publisherId);

            Assert.That(actionResult, Is.TypeOf<NotFoundResult>());
         
        }

        [Test, Order(4)]
        public void HTTPPOST_AddPublisher_ReturnsCreated_Test()
        {
            var newPublisherViewModel = new PublisherViewModel()
            {
                Name = "New Publisher"
            };

            IActionResult actionResult = publishersController.AddPublisher(newPublisherViewModel);
            Assert.That(actionResult, Is.TypeOf<CreatedResult>());
        }

        [Test, Order(5)]
        public void HTTPPOST_AddPublisher_ReturnsBadRequest_Test()
        {
            var newPublisherViewModel = new PublisherViewModel()
            {
                Name = "123 New Publisher"
            };

            IActionResult actionResult = publishersController.AddPublisher(newPublisherViewModel);
            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
        }
        [OneTimeTearDown]
        public void CleanUp()
        {
            context.Database.EnsureDeleted();
        }
        private void SeedDatabase()
        {
            var publishers = new List<Publisher>
            {
                new Publisher() { Id = 1, Name = "Publisher 1" },
                new Publisher() { Id = 2, Name = "Publisher 2" },
                new Publisher() { Id = 3, Name = "Publisher 3" },
                new Publisher() { Id = 4, Name = "Publisher 4" },
                new Publisher() { Id = 5, Name = "Publisher 5" },
                new Publisher() { Id = 6, Name = "Publisher 6" },
            };

            context.Publishers.AddRange(publishers);

            context.SaveChanges();
        }
    }
}
