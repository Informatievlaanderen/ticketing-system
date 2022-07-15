namespace TicketingService.ContainerHelper
{
    using Ductus.FluentDocker.Builders;
    using Ductus.FluentDocker.Services;

    public static class Container
    {
        public static ICompositeService Compose(string fileName, string waitForService, string waitForPort, string waitForProto)
        {
            return new Builder()
                .UseContainer()
                .UseCompose()
                .FromFile(fileName)
                .RemoveOrphans()
                .WaitForPort(waitForService, $"{waitForPort}:{waitForProto}", 30_000)
                .Build()
                .Start();
        }
    }
}
