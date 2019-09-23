using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Rakenne.Abstractions.Parsers.Implementation;
using Rakenne.Abstractions.Parsers.Interfaces;
using Rakenne.CosmosDB.Sql.Configurations;
using Rakenne.CosmosDB.Sql.Providers;
using Rakenne.CosmosDB.Sql.WatcherClients.Implementation;
using Rakenne.CosmosDB.Sql.WatcherClients.Interfaces;

namespace Rakenne.CosmosDB.Sql.Sources
{
    public sealed class CosmosDBConfigurationSource : IConfigurationSource, IDisposable
    {
        private readonly WebHostBuilderContext _context;
        private readonly CosmosDBConfiguration _configuration;

        private IWatcherClient _watcherClient;
        private IParser<string> _parser;

        public CosmosDBConfigurationSource(WebHostBuilderContext context, CosmosDBConfiguration configuration, CosmosClient client)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            _watcherClient = new CosmosDBWatcherClient(client, _context, _configuration);
            _parser = new JsonParser();
        }


        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new CosmosDBConfigurationProvider(_context, _configuration, _watcherClient, _parser);
        }

        public void Dispose()
        {
            _watcherClient.Dispose();
            _watcherClient = null;
            _parser = null;
        }
    }
}