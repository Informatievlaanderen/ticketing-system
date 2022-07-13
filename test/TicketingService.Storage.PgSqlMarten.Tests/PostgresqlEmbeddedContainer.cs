//namespace TicketingService.Storage.PgSqlMarten.Tests;

//using System;
//using ContainerHelper;
//using Npgsql;

//public class PostgresqlEmbeddedContainer : DockerContainer
//{
//    private const int HostPort = 21533;
//    private const string Password = "E@syP@ssw0rd";

//    public PostgresqlEmbeddedContainer(string connectionString)
//    {
//        Configuration = new SqlServerContainerConfiguration(connectionString);
//    }

//    private class SqlServerContainerConfiguration : DockerContainerConfiguration
//    {
//        public SqlServerContainerConfiguration(string connectionString)
//        {
//            Image = new ImageSettings
//            {
//                Registry = "hub.docker.com",
//                Name = "postgres",
//                Tag = "14.4"
//            };

//            Container = new ContainerSettings
//            {
//                Name = "roadregistry-api-db",
//                PortBindings = new[]
//                {
//                    new PortBinding
//                    {
//                        HostPort = HostPort,
//                        GuestPort = 1433
//                    }
//                },
//                EnvironmentVariables = new[]
//                {
//                    "ACCEPT_EULA=Y",
//                    $"SA_PASSWORD={Password}"
//                }
//            };

//            WaitUntilAvailable = async attempt =>
//            {
//                if (attempt <= 30)
//                {
//                    try
//                    {
//                        await using var connection = new NpgsqlConnection(connectionString);
//                        await connection.OpenAsync();
//                        connection.Close();

//                        return TimeSpan.Zero;
//                    }
//                    catch
//                    {
//                        // do nothing
//                    }

//                    return TimeSpan.FromSeconds(1);
//                }

//                throw new TimeoutException("The container {Container.Name} did not become available in a timely fashion.");
//            };
//        }
//    }
//}
