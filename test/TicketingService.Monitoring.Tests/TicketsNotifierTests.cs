namespace TicketingService.Monitoring.Tests;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Abstractions;
using Be.Vlaanderen.Basisregisters.GrAr.Notifications;
using FluentAssertions;
using Marten;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

public class MonitorsTests : IClassFixture<SetupMartenFixture>
{
    private readonly SetupMartenFixture _fixture;

    public MonitorsTests(SetupMartenFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task FilterTicketByRegistry()
    {
        const string registry = "TestRegistry";
        const string registry2 = "TestRegistry2";

        foreach (var i in Enumerable.Range(0, 5000))
        {
            await _fixture.MartenTicketing.CreateTicket(new Dictionary<string, string>
            {
                {MetaDataConstants.Registry, "otherregistry"},
                {MetaDataConstants.Action, "dummy"}
            });
        }

        var id = await _fixture.MartenTicketing.CreateTicket(new Dictionary<string, string>
        {
            {MetaDataConstants.Registry, registry},
            {MetaDataConstants.Action, "dummy"}
        });

        foreach (var i in Enumerable.Range(0, 10000))
        {
            await _fixture.MartenTicketing.CreateTicket(new Dictionary<string, string>
            {
                {MetaDataConstants.Registry, "otherregistry"},
                {MetaDataConstants.Action, "dummy"}
            });
        }

        await _fixture.MartenTicketing.CreateTicket(new Dictionary<string, string>
        {
            {MetaDataConstants.Registry, registry2},
            {MetaDataConstants.Action, "dummy"}
        });

        await _fixture.MartenTicketing.Pending(id);

        var registries = new string[] {registry, registry2};

        // var tickets = _fixture.DocumentStore.QuerySession().
        //     .Query<Ticket>()
        //     .Where(t => registries.Contains(t.Metadata[MetaDataConstants.Registry]))
        //     .ToList();

        var q = _fixture.DocumentStore.QuerySession()
            .Query<Ticket>($"select data " +
                           $"from mt_doc_ticket " +
                           $"where data -> 'metadata'->> 'registry' = '{registry}' " +
                           $"or data -> 'metadata'->> 'registry' = '{registry2}'")
            .Where(x => x.Status == TicketStatus.Pending)
            .ToList();

        var q2 = _fixture.DocumentStore.QuerySession()
            .Query<Ticket>($"select data from mt_doc_ticket where data ->> 'ticketId' = '{id}'")
            .ToList();


        // Assert
        q.Count.Should().Be(0);
        q2.Count.Should().Be(1);


    }

    [Fact]
    public async Task TicketInStatusCreated()
    {
        const string registry = "TestRegistry";
        var notificationService = new Mock<INotificationService>();

        // Create ticket. This signals the start of the first interval.
        var stopwatch = Stopwatch.StartNew();
        await Task.Delay(TimeSpan.FromMilliseconds(500)); // Let's wait a bit to enlarge the interval and give us some margin.

        await _fixture.MartenTicketing.CreateTicket(new Dictionary<string, string>
        {
            { MetaDataConstants.Registry, registry },
            { MetaDataConstants.Action, "dummy" }
        });

        var sut = new TicketsNotifier(
            _fixture.DocumentStore,
            notificationService.Object,
            new OptionsWrapper<NotificationsOptions>(new NotificationsOptions
            {
                WhitelistedRegistries = new[] { registry },
                BlacklistedActions = Array.Empty<string>()
            }));

        stopwatch.Stop();
        var interval = stopwatch.Elapsed.Add(TimeSpan.FromMilliseconds(500)); // Let's wait a bit to enlarge the interval and give us some margin.

        var timeToWaitForFirstAndSecondIntervalToPass = TimeSpan.FromMilliseconds(500).Add(interval);
        await Task.Delay(timeToWaitForFirstAndSecondIntervalToPass); // Let's way for the first and second interval to pass

        // Act
        await sut.OnTicketsOpenLongerThan(interval);

        // Assert
        notificationService.Verify(x => x.PublishToTopicAsync(It.IsAny<NotificationMessage>()), Times.Once);
    }

    [Fact]
    public async Task TicketInStatusPending()
    {
        const string registry = "TestRegistry";

        var notificationService = new Mock<INotificationService>();

        // Create and update ticket. This signals the start of the first interval.
        var stopwatch = Stopwatch.StartNew();
        await Task.Delay(TimeSpan.FromMilliseconds(500)); // Let's wait a bit to enlarge the interval and give us some margin.

        var ticketId = await _fixture.MartenTicketing.CreateTicket(new Dictionary<string, string>
        {
            { MetaDataConstants.Registry, registry },
            { MetaDataConstants.Action, "dummy" }
        });
        await _fixture.MartenTicketing.Pending(ticketId);

        var sut = new TicketsNotifier(
            _fixture.DocumentStore,
            notificationService.Object,
            new OptionsWrapper<NotificationsOptions>(new NotificationsOptions
            {
                WhitelistedRegistries = new[] { registry },
                BlacklistedActions = Array.Empty<string>()
            }));

        stopwatch.Stop();
        var interval = stopwatch.Elapsed.Add(TimeSpan.FromMilliseconds(500)); // Let's wait a bit to enlarge the interval and give us some margin.

        var timeToWaitForFirstAndSecondIntervalToPass = TimeSpan.FromMilliseconds(500).Add(interval);
        await Task.Delay(timeToWaitForFirstAndSecondIntervalToPass); // Let's way for the first and second interval to pass

        // Act
        await sut.OnTicketsOpenLongerThan(interval);

        // Assert
        notificationService.Verify(x => x.PublishToTopicAsync(It.IsAny<NotificationMessage>()), Times.Once);
    }
}
