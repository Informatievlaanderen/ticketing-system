namespace TicketingService.Monitoring.Tests
{
    using Xunit;

    [CollectionDefinition("PostgreSQL")]
    public class PostgreSQLCollection : ICollectionFixture<SetupMartenFixture>
    { }
}
