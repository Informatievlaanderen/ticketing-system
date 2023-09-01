namespace TicketingService.Monitoring.Tests;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Be.Vlaanderen.Basisregisters.GrAr.Notifications;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

[Collection("PostgreSQL")]
public class TicketsNotifierTests : IDisposable
{
    private readonly SetupMartenFixture _fixture;

    public TicketsNotifierTests(SetupMartenFixture fixture)
    {
        _fixture = fixture;
    }

    public void Dispose()
    { }

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
