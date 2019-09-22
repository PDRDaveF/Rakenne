using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Rakenne.CosmosDB.Sql.Configurations;

namespace Rakenne.CosmosDB.Sql
{
    public sealed class CosmosDBWatcherClient : IDisposable
    {
        private ConfigurationReloadToken _reloadToken = new ConfigurationReloadToken();

        private CosmosClient _cosmosClient;

        private readonly ChangeFeedProcessor _processor;


        public CosmosDBWatcherClient(WebHostBuilderContext context, CosmosDBConfiguration configuration)
        {
            _cosmosClient = new CosmosClient(configuration.ConnectionString, configuration.PrimaryKey);

            var leaseContainer = _cosmosClient.GetContainer(configuration.Database, "leases");

            var monitoredContainer = _cosmosClient.GetContainer(configuration.Database, context.HostingEnvironment.ApplicationName);

            var builder = monitoredContainer
                .GetChangeFeedProcessorBuilder<object>(string.Empty,
                    async (changes, token) => await ProcessChanges(token))
                .WithInstanceName($"{context.HostingEnvironment.ApplicationName}-{context.HostingEnvironment.EnvironmentName}")
                .WithLeaseContainer(leaseContainer)
                .WithPollInterval(new TimeSpan(0, 0, 0, 5));

            _processor = builder.Build();
        }

        private async Task ProcessChanges(CancellationToken token)
        {
            var previousToken = Interlocked.Exchange(ref _reloadToken, new ConfigurationReloadToken());
            previousToken.OnReload();
            await Task.Delay(1, token);
        }

        public IChangeToken Watch()
        {
            Task.Run(async () =>
            {
                await _processor.StartAsync();
            });

            return _reloadToken;
        }

        public void Dispose()
        {
            _cosmosClient.Dispose();
            _cosmosClient = null;
        }
    }
}
