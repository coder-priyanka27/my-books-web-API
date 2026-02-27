using Microsoft.AspNetCore.Mvc;
using my_books.Data.Models;
using my_books.Data.ViewModels;

namespace my_books.Data.Services
{
    public class BooksService
    {
        private AppDbContext _context;
        public BooksService(AppDbContext context)
        {
            _context = context;
        }

        public void AddBookWithAuthors(BookViewModel book)
        {
            var _book = new Book()
            {
                Title = book.Title,
                Description = book.Description,
                IsRead = book.IsRead,
                DateRead = book.IsRead ? book.DateRead : null,
                Genre = book.Genre,
                Rate = book.IsRead ? book.Rate.Value : null,
                CoverUrl = book.CoverUrl,
                DateAdded = DateTime.Now,
                //PublisherId = book.PublisherId
            };
            _context.Books.Add(_book);
            _context.SaveChanges();

            foreach (var id in book.AuthorNames)
            {
                var _book_author = new Book_Author()
                {
                    BookId = _book.Id,
                    AuthorId = int.Parse(id)
                };
                _context.Book_Authors.Add(_book_author);
                _context.SaveChanges();
            }
        }

        public List<Book> GetAllBooks() => _context.Books.ToList();

        public BookViewModel GetBookById(int bookId)
        {
            var _bookWithAuthors = _context.Books.Where(n => n.Id == bookId).Select(book => new BookViewModel()
            {
                Title = book.Title,
                Description = book.Description,
                IsRead = book.IsRead,
                DateRead = book.IsRead ? book.DateRead : null,
                Genre = book.Genre,
                Rate = book.IsRead ? book.Rate.Value : null,
                CoverUrl = book.CoverUrl,
                PublisherName = book.Publisher.Name,
                AuthorNames = book.Book_Authors.Select(n => n.Author.FullName).ToList()
            }).FirstOrDefault();

            return _bookWithAuthors;
        }

        public Book UpdateBookById(int bookId, BookViewModel book)
        {
            var _book = _context.Books.FirstOrDefault(n => n.Id == bookId);
            if(_book != null)
            {
                _book.Title = book.Title;
                _book.Description = book.Description;
                _book.IsRead = book.IsRead;
                _book.DateRead = book.IsRead ? book.DateRead : null;
                _book.Genre = book.Genre;
                _book.Rate = book.IsRead ? book.Rate.Value : null;
                _book.CoverUrl = book.CoverUrl;
                
                _context.SaveChanges();
            }
                return _book;
        }
        public void DeleteBookById(int bookId)
        {
            var _book = _context.Books.FirstOrDefault(n => n.Id == bookId);
            if(_book != null)
            {
                _context.Books.Remove(_book);
                _context.SaveChanges();
            }
        }
    }
}
