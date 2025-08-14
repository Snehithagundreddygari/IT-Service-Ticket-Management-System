using ItServiceTicketApi.Data;
using ItServiceTicketApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ItServiceTicketApi.Services
{
    public class TicketService : ITicketService
    {
        private readonly AppDbContext _db;
        public TicketService(AppDbContext db) { _db = db; }

        public async Task<Ticket> CreateTicketAsync(Ticket t)
        {
            t.TicketNumber = GenerateTicketNumber();
            if (t.SlaPolicyId.HasValue)
            {
                t.SlaPolicy = await _db.SlaPolicies.FindAsync(t.SlaPolicyId.Value);
                t.ApplySlaDates();
            }
            t.CreatedAt = DateTime.UtcNow;
            t.UpdatedAt = DateTime.UtcNow;
            _db.Tickets.Add(t);
            await _db.SaveChangesAsync();
            return t;
        }

        public async Task<List<Ticket>> GetOpenTicketsAsync()
        {
            return await _db.Tickets
                .Include(t => t.Customer)
                .Include(t => t.AssignedToUser)
                .Include(t => t.SlaPolicy)
                .Where(t => t.Status != TicketStatus.Closed && t.Status != TicketStatus.Resolved)
                .OrderBy(t => t.Priority)
                .ThenBy(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<Ticket?> GetTicketAsync(long id)
        {
            return await _db.Tickets
                .Include(t => t.Comments)
                .Include(t => t.AssignedToUser)
                .Include(t => t.Customer)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AssignTicketAsync(long ticketId, int userId, int? queueId)
        {
            var t = await _db.Tickets.FindAsync(ticketId);
            if (t == null) throw new KeyNotFoundException("Ticket not found");
            t.AssignedToId = userId;
            t.QueueId = queueId;
            t.Status = TicketStatus.Open;
            t.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }

        public async Task AddCommentAsync(long ticketId, TicketComment comment)
        {
            comment.TicketId = ticketId;
            comment.CreatedAt = DateTime.UtcNow;
            _db.TicketComments.Add(comment);
            await _db.SaveChangesAsync();
        }

        public async Task<int> EscalatePastSlaAsync()
        {
            var now = DateTime.UtcNow;
            var tickets = await _db.Tickets
                .Include(t => t.SlaPolicy)
                .Where(t => (t.Status == TicketStatus.New || t.Status == TicketStatus.Open || t.Status == TicketStatus.InProgress || t.Status == TicketStatus.OnHold)
                            && t.SlaResolutionDue != null && t.SlaResolutionDue < now)
                .ToListAsync();

            int count = 0;
            foreach (var t in tickets)
            {
                // bump priority
                if (t.Priority > TicketPriority.P1_Critical)
                {
                    t.Priority = (TicketPriority)((int)t.Priority - 1);
                    t.Status = TicketStatus.Escalated;
                    t.UpdatedAt = DateTime.UtcNow;
                    _db.TicketComments.Add(new TicketComment
                    {
                        TicketId = t.Id,
                        CommentText = $"Auto-escalated due to SLA breach. New priority: {t.Priority}",
                        Internal = true
                    });
                    count++;
                }
            }
            if (count > 0) await _db.SaveChangesAsync();
            return count;
        }

        private string GenerateTicketNumber()
        {
            return "TCKT-" + DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
        }
    }
}
