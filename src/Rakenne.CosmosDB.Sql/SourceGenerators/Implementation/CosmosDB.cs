using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Rakenne.Abstractions.SourceGenerators;
using Rakenne.Abstractions.SourceGenerators.Interfaces;
using Rakenne.CosmosDB.Sql.Configurations;
using Rakenne.CosmosDB.Sql.Sources;

namespace Rakenne.CosmosDB.Sql.SourceGenerators.Implementation
{
    public sealed class CosmosDB : ConfigurationSourceGeneratorBase<CosmosDB>, IConfigurationSourceGenerator, IDisposable
    {
        private CosmosClient _client;

        public IConfigurationSource Create(IConfigurationBuilder configurationBuilder, WebHostBuilderContext context)
        {
            var configuration = GetConfiguration<CosmosDBConfiguration>(configurationBuilder);
            _client = new CosmosClient(configuration.ConnectionString, configuration.PrimaryKey);
            return new CosmosDBConfigurationSource(context, configuration, _client);
        }

        public void Dispose()
        {
            _client.Dispose();
            _client = null;
        }
    }
}