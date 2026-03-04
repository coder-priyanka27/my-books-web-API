namespace my_books.Data.ViewModels
{
    public class AuthorViewModel
    {
        public string FullName { get; set; }
    }
    public class AuthorWithBooksViewModel
    {
        public string FullName { get; set; }
        public List<string> BookTitles { get; set; }
    }
}
