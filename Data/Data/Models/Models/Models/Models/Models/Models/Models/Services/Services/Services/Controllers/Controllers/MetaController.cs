using ItServiceTicketApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ItServiceTicketApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MetaController : ControllerBase
    {
        private readonly AppDbContext _db;
        public MetaController(AppDbContext db) { _db = db; }

        [HttpGet("users")] public async Task<IActionResult> Users() => Ok(await _db.Users.ToListAsync());
        [HttpGet("queues")] public async Task<IActionResult> Queues() => Ok(await _db.Queues.ToListAsync());
        [HttpGet("slas")] public async Task<IActionResult> Slas() => Ok(await _db.SlaPolicies.ToListAsync());
        [HttpGet("customers")] public async Task<IActionResult> Customers() => Ok(await _db.Customers.ToListAsync());
    }
}
