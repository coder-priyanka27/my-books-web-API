using Microsoft.EntityFrameworkCore.Metadata;

namespace my_books.Data.ViewModels
{
    public class BookViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsRead { get; set; }
        public DateTime? DateRead { get; set; }
        public string Genre { get; set; }
        public int? Rate { get; set; }
        public string CoverUrl { get; set; }

        public int PublisherId { get; set; }
        public List<int> AuthorIds { get; set; }
    }
}
