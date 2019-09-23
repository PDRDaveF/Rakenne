using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Rakenne.Abstractions.Extensions;
using Rakenne.CosmosDB.Sql.Configurations;
using Rakenne.CosmosDB.Sql.WatcherClients.Interfaces;

namespace Rakenne.CosmosDB.Sql.WatcherClients.Implementation
{
    public sealed class CosmosDBWatcherClient : IWatcherClient
    {
        private ConfigurationReloadToken _reloadToken = new ConfigurationReloadToken();

        private CosmosClient _cosmosClient;

        private readonly ChangeFeedProcessor _processor;


        public CosmosDBWatcherClient(CosmosClient cosmosClient, WebHostBuilderContext context, CosmosDBConfiguration configuration)
        {
            _cosmosClient = cosmosClient ?? throw new ArgumentNullException(nameof(cosmosClient));

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (string.IsNullOrWhiteSpace(configuration.Database))
            {
                throw new ArgumentNullException(nameof(configuration), "Database is missing from configuration");
            }

            if (string.IsNullOrWhiteSpace(configuration.LeaseContainerName))
            {
                throw new ArgumentNullException(nameof(configuration), "Lease container name is missing from configuration");
            }

            var leaseContainer = _cosmosClient.GetContainer(configuration.Database, configuration.LeaseContainerName);

            var monitoredContainer = _cosmosClient.GetContainer(configuration.Database, configuration.GetDataSourceName(context.HostingEnvironment));

            var builder = monitoredContainer
                .GetChangeFeedProcessorBuilder<object>(string.Empty,
                    async (changes, token) => await ProcessChanges(token))
                .WithInstanceName($"{configuration.GetDataSourceName(context.HostingEnvironment)}-{configuration.GetEnvironment(context.HostingEnvironment)}")
                .WithLeaseContainer(leaseContainer);

            if (configuration.PollingIntervalInSeconds < 1)
            {
                _processor = builder.Build();
                return;
            }

            builder.WithPollInterval(new TimeSpan(0, 0, 0, configuration.PollingIntervalInSeconds));
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
