using ItServiceTicketApi.Models;
using ItServiceTicketApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ItServiceTicketApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _svc;
        public TicketsController(ITicketService svc) { _svc = svc; }

        [HttpGet]
        public async Task<IActionResult> GetOpen() => Ok(await _svc.GetOpenTicketsAsync());

        [HttpGet("{id:long}")]
        public async Task<IActionResult> Get(long id)
        {
            var t = await _svc.GetTicketAsync(id);
            if (t == null) return NotFound();
            return Ok(t);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Ticket t)
        {
            var created = await _svc.CreateTicketAsync(t);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPost("{id:long}/assign")]
        public async Task<IActionResult> Assign(long id, [FromQuery] int userId, [FromQuery] int? queueId)
        {
            await _svc.AssignTicketAsync(id, userId, queueId);
            return NoContent();
        }

        [HttpPost("{id:long}/comments")]
        public async Task<IActionResult> AddComment(long id, [FromBody] TicketComment comment)
        {
            await _svc.AddCommentAsync(id, comment);
            return NoContent();
        }
    }
}
