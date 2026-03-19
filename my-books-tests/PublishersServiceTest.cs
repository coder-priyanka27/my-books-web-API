using Microsoft.EntityFrameworkCore;
using my_books.Data;
using my_books.Data.Models;

namespace my_books_tests
{
    public class PublishersServiceTest
    {
        private static DbContextOptions<AppDbContext> dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "BookDbTest")
            .Options;

        AppDbContext context;

        [OneTimeSetUp]
        public void Setup()
        {
            context = new AppDbContext(dbContextOptions);
            context.Database.EnsureCreated();

            SeedDatabase();
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
                new Publisher() { Id = 1, Name = "Publisher One" },
                new Publisher() { Id = 2, Name = "Publisher Two" },
                new Publisher() { Id = 3, Name = "Publisher Three" },
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