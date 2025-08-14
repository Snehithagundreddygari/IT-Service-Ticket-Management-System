namespace ItServiceTicketApi.Models
{
    public enum TicketPriority { P1_Critical = 1, P2_High = 2, P3_Medium = 3, P4_Low = 4 }

    public class SlaPolicy
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public TicketPriority Priority { get; set; } = TicketPriority.P3_Medium;
        public int ResponseHours { get; set; } = 2;
        public int ResolutionHours { get; set; } = 48;
    }
}
