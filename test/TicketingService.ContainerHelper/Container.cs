namespace TicketingService.ContainerHelper;

using System;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;

public static class Container
{
    public static ICompositeService Compose(string fileName, string waitForService, string waitForPort, string waitForProto, TimeSpan? timeout = null)
    {
        var timeoutMs = timeout?.TotalMilliseconds ?? 30_000;
        
        // docker-compose -f postgres_test.yml up
        return new Builder()
            .UseContainer()
            .UseCompose()
            .FromFile(fileName)
            .RemoveOrphans()
            .WaitForPort(waitForService, $"{waitForPort}:{waitForProto}", Convert.ToInt64(timeoutMs))
            .Build()
            .Start();
    }
}
