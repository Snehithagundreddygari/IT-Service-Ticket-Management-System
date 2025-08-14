using System.ComponentModel.DataAnnotations.Schema;

namespace ItServiceTicketApi.Models
{
    public enum TicketStatus { New, Open, InProgress, OnHold, Resolved, Closed, Escalated }

    public class Ticket
    {
        public long Id { get; set; }
        public string TicketNumber { get; set; } = "";
        public string Title { get; set; } = "";
        public string? Description { get; set; }
        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public int? CreatedById { get; set; }
        public User? CreatedBy { get; set; }
        public int? AssignedToId { get; set; }
        public User? AssignedToUser { get; set; }
        public int? QueueId { get; set; }
        public Queue? Queue { get; set; }
        public TicketPriority Priority { get; set; } = TicketPriority.P3_Medium;
        public TicketStatus Status { get; set; } = TicketStatus.New;
        public string? Source { get; set; }
        public int? SlaPolicyId { get; set; }
        public SlaPolicy? SlaPolicy { get; set; }
        public DateTime? SlaResponseDue { get; set; }
        public DateTime? SlaResolutionDue { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public List<TicketComment>? Comments { get; set; }

        // helper to compute SLA dates
        public void ApplySlaDates()
        {
            if (SlaPolicy == null) return;
            var now = DateTime.UtcNow;
            SlaResponseDue = now.AddHours(SlaPolicy.ResponseHours);
            SlaResolutionDue = now.AddHours(SlaPolicy.ResolutionHours);
        }
    }
}
