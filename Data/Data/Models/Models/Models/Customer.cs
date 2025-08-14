namespace ItServiceTicketApi.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? ContactEmail { get; set; }
        public string? ContactPhone { get; set; }
        public string? Company { get; set; }
    }
}
