using Backend_Chat.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Backend_Chat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MessagingContext _context;

        public UsersController(MessagingContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _context.Users.ToList();
            return Ok(users);
        }
    }
}
