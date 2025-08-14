namespace ItServiceTicketApi.Models
{
    public class TicketComment
    {
        public long Id { get; set; }
        public long TicketId { get; set; }
        public Ticket? Ticket { get; set; }
        public int? AuthorId { get; set; }
        public User? Author { get; set; }
        public string CommentText { get; set; } = "";
        public bool Internal { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
