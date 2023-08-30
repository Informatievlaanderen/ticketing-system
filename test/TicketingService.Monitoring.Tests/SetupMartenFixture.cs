namespace TicketingService.Monitoring.Tests
{
    using System;
    using System.Threading.Tasks;
    using Be.Vlaanderen.Basisregisters.DockerUtilities;
    using Ductus.FluentDocker.Services;
    using Marten;
    using Microsoft.Extensions.DependencyInjection;
    using Storage.PgSqlMarten;

    public class SetupMartenFixture : IDisposable
    {
        private ICompositeService _container;
        public IDocumentStore DocumentStore { get; private set; }
        public MartenTicketing MartenTicketing { get; private set; }

        public SetupMartenFixture()
        {
            var port = "3435";
            Environment.SetEnvironmentVariable("PORT", port);
            _container = DockerComposer.Compose("postgres_test.yml", "ticketing-system-monitoring-integration-tests");

            var services = new ServiceCollection();
            services.AddMartenTicketing($"Host=localhost;Port={port};Database=tickets;Username=postgres;Password=postgres;Include Error Detail=True;");
            var serviceProvider = services.BuildServiceProvider();

            var retries = 0;
            while (_container.State != ServiceRunningState.Running && retries < 6)
            {
                Task.Delay(1000);
                retries++;
            }

            DocumentStore = serviceProvider.GetRequiredService<IDocumentStore>();
            MartenTicketing = new MartenTicketing(DocumentStore);
        }

        public void Dispose()
        {
            _container.Dispose();
        }
    }
}
