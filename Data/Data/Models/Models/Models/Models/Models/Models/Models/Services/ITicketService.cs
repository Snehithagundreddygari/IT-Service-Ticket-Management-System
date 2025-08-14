using ItServiceTicketApi.Models;

namespace ItServiceTicketApi.Services
{
    public interface ITicketService
    {
        Task<Ticket> CreateTicketAsync(Ticket t);
        Task<List<Ticket>> GetOpenTicketsAsync();
        Task<Ticket?> GetTicketAsync(long id);
        Task AssignTicketAsync(long ticketId, int userId, int? queueId);
        Task AddCommentAsync(long ticketId, TicketComment comment);
        Task<int> EscalatePastSlaAsync();
    }
}
