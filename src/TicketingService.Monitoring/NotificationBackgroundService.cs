namespace TicketingService.Monitoring
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class NotificationsOptions
    {
        public string[] WhitelistedRegistries { get; set; }
        public string[] BlacklistedActions { get; set; }
    }

    public class NotificationBackgroundService :  IHostedService, IDisposable
    {
        private static readonly TimeSpan Interval = TimeSpan.FromMinutes(5);

        private readonly TicketsNotifier _ticketsNotifier;
        private readonly ILogger _logger;

        private Timer? _timer;

        public NotificationBackgroundService(
            ILoggerFactory loggerFactory,
            TicketsNotifier notifier)
        {
            _ticketsNotifier = notifier;
            _logger = loggerFactory.CreateLogger<NotificationBackgroundService>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Starting notification background service, monitoring every {Interval:g}.");
            _timer = new Timer(DoWork, null, TimeSpan.Zero, Interval);

            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            _ticketsNotifier.OnTicketsOpenLongerThan(Interval).Wait();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
           _logger.LogInformation("Stopping notification background service.");
           _timer?.Change(Timeout.Infinite, 0);

           return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
