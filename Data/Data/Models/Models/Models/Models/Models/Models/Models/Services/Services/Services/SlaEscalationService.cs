using Microsoft.Extensions.Hosting;

namespace ItServiceTicketApi.Services
{
    public class SlaEscalationService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<SlaEscalationService> _logger;

        public SlaEscalationService(IServiceScopeFactory scopeFactory, ILogger<SlaEscalationService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("SLA Escalation Service started.");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var svc = scope.ServiceProvider.GetRequiredService<ITicketService>();
                    var count = await svc.EscalatePastSlaAsync();
                    if (count > 0) _logger.LogInformation("Escalated {count} tickets due to SLA breach.", count);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error running SLA escalation.");
                }

                await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken); // run every 60s (adjust for demo)
            }
        }
    }
}
