namespace TicketingService.Monitoring.Tests
{
    using System;
    using System.Threading.Tasks;
    using Be.Vlaanderen.Basisregisters.DockerUtilities;
    using Ductus.FluentDocker.Services;
    using Marten;
    using Microsoft.Extensions.DependencyInjection;
    using Npgsql;
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

            CreateDatabase(
                $"Host=localhost;Port={port};Username=postgres;Password=postgres;Include Error Detail=True;",
                "tickets").GetAwaiter().GetResult();

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

        private async Task CreateDatabase(string connectionString, string database)
        {
            var createDbQuery = $"CREATE DATABASE {database}";

            await using var connection = new NpgsqlConnection(connectionString);
            await using var command = new NpgsqlCommand(createDbQuery, connection);

            var attempt = 1;
            while (attempt <= 5)
            {
                try
                {
                    await connection.OpenAsync();
                }
                catch (Exception)
                {
                    attempt++;
                    await Task.Delay(TimeSpan.FromMilliseconds(200));
                }
            }

            await command.ExecuteNonQueryAsync();
        }

        public void Dispose()
        {
            _container.Dispose();
        }
    }
}
