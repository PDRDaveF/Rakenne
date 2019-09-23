﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
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
            _cosmosClient = cosmosClient;

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
