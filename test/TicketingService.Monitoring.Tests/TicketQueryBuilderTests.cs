namespace TicketingService.Monitoring.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Abstractions;
    using FluentAssertions;
    using Xunit;

    [Collection("PostgreSQL")]
    public class TicketQueryBuilderTests : IDisposable
    {
        private readonly SetupMartenFixture _fixture;

        public TicketQueryBuilderTests(SetupMartenFixture fixture)
        {
            _fixture = fixture;
        }

        public void Dispose()
        {
            var session =_fixture.DocumentStore.DirtyTrackedSession();
            session.DeleteWhere<Ticket>(_ => true);
            session.SaveChanges();
        }

        [Fact]
        public async Task FilterOn_DateRegistriesPaged()
        {
            await _fixture.MartenTicketing.CreateTicket(new Dictionary<string, string>
            {
                {MetaDataConstants.Registry, "registry_1"}
            });
            await _fixture.MartenTicketing.CreateTicket(new Dictionary<string, string>
            {
                {MetaDataConstants.Registry, "registry_2"}
            });
            var id =await _fixture.MartenTicketing.CreateTicket(new Dictionary<string, string>
            {
                {MetaDataConstants.Registry, "registry_2"}
            });
            await _fixture.MartenTicketing.Pending(id, CancellationToken.None);
            await _fixture.MartenTicketing.CreateTicket(new Dictionary<string, string>
            {
                {MetaDataConstants.Registry, "registry_3"}
            });

            var from = DateTimeOffset.Now.Subtract(TimeSpan.FromMinutes(1));
            var to = DateTimeOffset.Now.Add(TimeSpan.FromMinutes(1));

            var query = new TicketQueryBuilder(_fixture.DocumentStore)
                .FromTo(Operators.WHERE, from.ToString(), to.ToString())
                .WhitelistedRegistries(Operators.AND, "registry_1", "registry_2")
                .ByStatuses(Operators.AND, new [] {TicketStatus.Created, TicketStatus.Pending})
                .Paged(0, 2);

            var result = await query.Execute();

            // Assert
            result.Count.Should().Be(2);
        }

        [Fact]
        public async Task FilterOn_Registries()
        {
            await _fixture.MartenTicketing.CreateTicket(new Dictionary<string, string>
            {
                {MetaDataConstants.Registry, "test"}
            });

            var result = await new TicketQueryBuilder(_fixture.DocumentStore)
                .WhitelistedRegistries(Operators.WHERE, "test")
                .Execute();

            // Assert
            result.Count.Should().Be(1);
        }

        [Fact]
        public async Task FilterOn_Date()
        {
            await _fixture.MartenTicketing.CreateTicket(new Dictionary<string, string>());

            var from = DateTimeOffset.Now.Subtract(TimeSpan.FromMinutes(1));
            var to = DateTimeOffset.Now.Add(TimeSpan.FromMinutes(1));

            var query = new TicketQueryBuilder(_fixture.DocumentStore)
                .FromTo(Operators.WHERE, from.ToString(), to.ToString());

            var result = await query.Execute();

            // Assert
            result.Count.Should().Be(1);
        }

        [Fact]
        public async Task FilterOn_Page()
        {
            await _fixture.MartenTicketing.CreateTicket(new Dictionary<string, string>());
            await _fixture.MartenTicketing.CreateTicket(new Dictionary<string, string>());
            await _fixture.MartenTicketing.CreateTicket(new Dictionary<string, string>());

            var result = await new TicketQueryBuilder(_fixture.DocumentStore)
                .Paged(0, 2)
                .Execute();

            // Assert
            result.Count.Should().Be(2);
        }

        [Fact]
        public async Task FilterOn_Status()
        {
            var id1 = await _fixture.MartenTicketing.CreateTicket(new Dictionary<string, string>());
            var id2 = await _fixture.MartenTicketing.CreateTicket(new Dictionary<string, string>());

            await _fixture.MartenTicketing.Pending(id1);
            await _fixture.MartenTicketing.Error(id2, new TicketError());

            await _fixture.MartenTicketing.CreateTicket(new Dictionary<string, string>());

            var query = new TicketQueryBuilder(_fixture.DocumentStore)
                .ByStatuses(Operators.WHERE, new[] { TicketStatus.Pending, TicketStatus.Error});

            var result = await query.Execute();

            // Assert
            result.Count.Should().Be(2);
        }
    }
}
