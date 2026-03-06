using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using my_books.Data.Services;
using my_books.Data.ViewModels;

namespace my_books.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private PublishersService _publishersService;
        public PublishersController(PublishersService publishersService)
        {
                _publishersService = publishersService;
        }
        [HttpPost("add-publisher")]
        public IActionResult AddPublisher([FromBody] PublisherViewModel publisher)
        {
            var newPublisher = _publishersService.AddPublisher(publisher);
            return Created(nameof(AddPublisher), newPublisher);
        }
        [HttpGet("get-publisher-bt-id/{id}")]
        public IActionResult GetPublisherById(int id)
        {
            var _publisherData = _publishersService.GetPublisherById(id);

            if (_publisherData != null)
            {
                return Ok(_publisherData);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("delete-publisher-by-id/{id}")]
        public IActionResult DeletePublisherById(int id)
        {
            _publishersService.DeletePublisherById(id);
            return Ok();
        }
    }
}
