using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace my_books.Data.ViewModels
{
    public class ErrorViewModel
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Path { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

    }
}
