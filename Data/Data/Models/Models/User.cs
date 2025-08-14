using System.ComponentModel.DataAnnotations;

namespace ItServiceTicketApi.Models
{
    public class User
    {
        public int Id { get; set; }
        [MaxLength(80)] public string Username { get; set; } = "";
        [MaxLength(150)] public string FullName { get; set; } = "";
        public string? Email { get; set; }
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
        public bool IsAgent { get; set; }
        public bool IsManager { get; set; }

        public List<Ticket>? AssignedTickets { get; set; }
    }
}
