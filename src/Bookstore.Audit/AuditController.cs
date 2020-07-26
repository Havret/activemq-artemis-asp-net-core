using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Audit
{
    [ApiController]
    [Route("[controller]")]
    public class AuditController : ControllerBase
    {
        private readonly BookstoreAuditContext _context;

        public AuditController(BookstoreAuditContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userActions = await _context.UserActions.ToArrayAsync();
            return Ok(userActions);
        }
    }
}