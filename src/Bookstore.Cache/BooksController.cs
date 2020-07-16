using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Cache
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly BookCache _cache;

        public BooksController(BookCache cache)
        {
            _cache = cache;
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var bookCacheEntry = await _cache.GetAsync(id);
            if (bookCacheEntry != null)
                return Ok(bookCacheEntry);
            else
                return NotFound();
        }
    }
}