namespace TicketingService.Monitoring.Tests
{
    using System.Threading.Tasks;
    using Be.Vlaanderen.Basisregisters.DockerUtilities;
    using Ductus.FluentDocker.Services;
    using Marten;
    using Microsoft.Extensions.DependencyInjection;
    using Storage.PgSqlMarten;
    using Xunit;

    public class SetupMartenFixture : IAsyncLifetime
    {
        private ICompositeService _container;
        public IDocumentStore DocumentStore { get; private set; }
        public MartenTicketing MartenTicketing { get; private set; }

        public Task InitializeAsync()
        {
            _container = DockerComposer.Compose("postgres_test.yml", "ticketing-system-monitoring-integration-tests");
            const string connectionString =
                "Host=localhost;Port=5435;Database=tickets;Username=postgres;Password=postgres;Include Error Detail=True;";

            var services = new ServiceCollection();
            services.AddMartenTicketing(connectionString);
            var serviceProvider = services.BuildServiceProvider();

            DocumentStore = serviceProvider.GetRequiredService<IDocumentStore>();
            MartenTicketing = new MartenTicketing(DocumentStore);

            return Task.CompletedTask;
        }

        public Task DisposeAsync()
        {
            _container.Dispose();
            return Task.CompletedTask;
        }
    }
}
