using my_books.Data.Models;
using my_books.Data.ViewModels;
using my_books.Exceptions;
using System.Text.RegularExpressions;

namespace my_books.Data.Services
{
    public class PublishersService
    {
        private AppDbContext _context;
        public PublishersService(AppDbContext context)
        {
            _context = context;
        }
        public Publisher AddPublisher(PublisherViewModel publisher)
        {
            if (StringStartsWithNumber(publisher.Name))
            {
                throw new PublisherNameException("Publisher name cannot start with a number", publisher.Name);
            }
            var _publisher = new Publisher()
            {
                Name = publisher.Name
            };
            _context.Publishers.Add(_publisher);
            _context.SaveChanges();

            return _publisher;
        }

        public Publisher GetPublisherById(int id) => _context.Publishers.FirstOrDefault(n => n.Id == id);
        public PublisherWithBooksAndAuthorsViewModel GetPublisherData(int publisherId)
        {
            var _publisherData = _context.Publishers.Where(n => n.Id == publisherId).Select(n => new PublisherWithBooksAndAuthorsViewModel()
            {
                Name = n.Name,
                BookAuthors = n.Books.Select(n => new BookAuthorViewModel()
                {
                    BookName = n.Title,
                    BookAuthors = n.Book_Authors.Select(n => n.Author.FullName).ToList()
                }).ToList()
            }).FirstOrDefault();
            return _publisherData;
        }

        public void DeletePublisherById(int id)
        {
            var _publisher = _context.Publishers.FirstOrDefault(n => n.Id == id);

            if(_publisher != null)
            {
                _context.Publishers.Remove(_publisher);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception($"The publisher with id: {id} does not exist");
            }
        }

        private bool StringStartsWithNumber(string name) => (Regex.IsMatch(name, @"^\d"));
    }
}