using System;
using System.Net;
using System.Threading.Tasks;
using Bookstore.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly BookstoreContext _context;
        private readonly MessageProducer _messageProducer;

        public BooksController(BookstoreContext context, MessageProducer messageProducer)
        {
            _context = context;
            _messageProducer = messageProducer;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateBook command)
        {
            var newBook = new Book
            {
                Id = Guid.NewGuid(),
                Title = command.Title,
                Author = command.Author,
                Cost = command.Cost,
                InventoryAmount = command.InventoryAmount,
            };
            await _context.Books.AddAsync(newBook);
            
            var @event = new BookCreated
            {
                Id = newBook.Id,
                Title = newBook.Title,
                Author = newBook.Author,
                Cost = newBook.Cost,
                InventoryAmount = newBook.InventoryAmount
            };
            await _messageProducer.PublishAsync(@event);
            
            return StatusCode((int) HttpStatusCode.Created, new { newBook.Id });
        }
    }
}