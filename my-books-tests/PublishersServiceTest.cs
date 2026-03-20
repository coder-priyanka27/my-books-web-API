using Microsoft.EntityFrameworkCore;
using my_books.Data;
using my_books.Data.Models;
using my_books.Data.Services;
using my_books.Data.ViewModels;
using my_books.Exceptions;

namespace my_books_tests
{
    public class PublishersServiceTest
    {
        private static DbContextOptions<AppDbContext> dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "BookDbTest")
            .Options;

        AppDbContext context;
        PublishersService publishersService;

        [OneTimeSetUp]
        public void Setup()
        {
            context = new AppDbContext(dbContextOptions);
            context.Database.EnsureCreated();

            SeedDatabase();

            publishersService = new PublishersService(context);
        }

        [Test, Order(1)]
        public void GetAllPublishers_WithNoSortBy_WithNoSearchString_WithNoPageNumber_Test()
        {
            var result = publishersService.GetAllPublishers("", "", null);

            Assert.That(result.Count, Is.EqualTo(5));
        }
        [Test, Order(2)]
        public void GetAllPublishers_WithNoSortBy_WithNoSearchString_WithPageNumber_Test()
        {
            var result = publishersService.GetAllPublishers("", "", 2);

            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test, Order(3)]
        public void GetAllPublishers_WithNoSortBy_WithSearchString_WithNoPageNumber_Test()
        {
            var result = publishersService.GetAllPublishers("", "3", null);

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.FirstOrDefault().Name, Is.EqualTo("Publisher 3"));
        }

        [Test, Order(4)]
        public void GetAllPublishers_WithSortBy_WithNoSearchString_WithNoPageNumber_Test()
        {
            var result = publishersService.GetAllPublishers("name_desc", "", null);

            Assert.That(result.Count, Is.EqualTo(5));
            Assert.That(result.FirstOrDefault().Name, Is.EqualTo("Publisher 6"));
        }

        [Test, Order(5)]
        public void GetPublisherById_WithResponse_Test()
        {
            var result = publishersService.GetPublisherById(1);

            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Name, Is.EqualTo("Publisher 1"));

        }

        [Test, Order(6)]
        public void GetPublisherById_WithoutResponse_Test()
        {
            var result = publishersService.GetPublisherById(25);

            Assert.That(result, Is.Null);
        }

        [Test, Order(7)]
        public void AddPublisher_WithException_Test()
        {
            var newPublisher = new PublisherViewModel()
            {
                Name = "123 With Exception"
            };

            Assert.That(() => publishersService.AddPublisher(newPublisher), Throws.Exception.TypeOf<PublisherNameException>().With.Message.EqualTo("Publisher name cannot start with a number"));
        }

        [Test, Order(8)]
        public void AddPublisher_WithoutException_Test()
        {
            var newPublisher = new PublisherViewModel()
            {
                Name = "Without Exception"
            };

            var result = publishersService.AddPublisher(newPublisher);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Does.StartWith("Without"));
            //Assert.That(result.Id, Is.Not.Null);
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

            var authors = new List<Author>
            {
                new Author() { Id = 1, FullName = "Author One" },
                new Author() { Id = 2, FullName = "Author Two" },
            };
            context.Authors.AddRange(authors);

            var books = new List<Book>
            {
                new Book() 
                { 
                    Id = 1, 
                    Title = "Book One",
                    Description = "Description for Book One",
                    IsRead = false,
                    Genre = "Fiction",
                    CoverUrl = "http://...",
                    DateAdded = DateTime.Now.AddDays(-10),
                    PublisherId = 1 
                },
                new Book()
                {
                    Id = 2,
                    Title = "Book Two",
                    Description = "Description for Book Two",
                    IsRead = false,
                    Genre = "Genre",
                    CoverUrl = "http://...",
                    DateAdded = DateTime.Now.AddDays(-10),
                    PublisherId = 1
                },
            };
            context.Books.AddRange(books);
             
            var bookAuthors = new List<Book_Author>
            {
                new Book_Author() { Id = 1, BookId = 1, AuthorId = 1 },
                new Book_Author() { Id = 2, BookId = 1, AuthorId = 2 },
                new Book_Author() { Id = 3, BookId = 2, AuthorId = 2 },
            };

            context.Book_Authors.AddRange(bookAuthors);

            context.SaveChanges();
        }  
    }
}