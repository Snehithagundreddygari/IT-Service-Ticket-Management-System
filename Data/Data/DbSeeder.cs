using ItServiceTicketApi.Models;

namespace ItServiceTicketApi.Data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext db)
        {
            if (db.Departments.Any()) return;

            var d1 = new Department { Name = "Service Desk" };
            var d2 = new Department { Name = "Infrastructure" };
            db.Departments.AddRange(d1, d2);

            var admin = new User { Username = "admin", FullName = "Service Admin", Email = "admin@example.com", IsAgent = true, IsManager = true, Department = d1 };
            var agent = new User { Username = "agent1", FullName = "Agent One", Email = "agent1@example.com", IsAgent = true, Department = d1 };
            db.Users.AddRange(admin, agent);

            var custA = new Customer { Name = "Alpha Ltd", ContactEmail = "alpha@alpha.com" };
            var custB = new Customer { Name = "Beta Inc", ContactEmail = "beta@beta.com" };
            db.Customers.AddRange(custA, custB);

            var qGeneral = new Queue { Name = "General Queue", Department = d1 };
            db.Queues.Add(qGeneral);

            var slaStandard = new SlaPolicy { Name = "Standard", Priority = TicketPriority.P3_Medium, ResponseHours = 2, ResolutionHours = 48 };
            var slaHigh = new SlaPolicy { Name = "High", Priority = TicketPriority.P2_High, ResponseHours = 1, ResolutionHours = 8 };
            db.SlaPolicies.AddRange(slaStandard, slaHigh);

            db.SaveChanges();

            // add sample tickets
            var t1 = new Ticket
            {
                TicketNumber = GenerateTicketNumber(),
                Title = "Unable to login",
                Description = "User cannot login to portal",
                Customer = custA,
                CreatedById = admin.Id,
                Queue = qGeneral,
                Priority = TicketPriority.P2_High,
                SlaPolicy = slaHigh
            };
            t1.ApplySlaDates();
            db.Tickets.Add(t1);

            var t2 = new Ticket
            {
                TicketNumber = GenerateTicketNumber(),
                Title = "Email delivery failure",
                Description = "Transactional emails bouncing",
                Customer = custB,
                CreatedById = agent.Id,
                Priority = TicketPriority.P1_Critical,
                SlaPolicy = slaHigh
            };
            t2.ApplySlaDates();
            db.Tickets.Add(t2);

            db.SaveChanges();
        }

        static string GenerateTicketNumber()
        {
            return "TCKT-" + DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
        }
    }
}
