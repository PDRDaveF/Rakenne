using System;
using Microsoft.AspNetCore.Hosting;
using MongoDB.Bson;
using MongoDB.Driver;
using Rakenne.Abstractions.Models;
using Rakenne.Abstractions.Parsers.Interfaces;
using Rakenne.Mongo.Abstractions.Providers;
using Rakenne.Mongo.Abstractions.Repository.Interfaces;
using Rakenne.Mongo.Tenanted.Configurations;

namespace Rakenne.Mongo.Tenanted.Providers.Implementation
{
    public sealed class MongoTenantedConfigurationProvider : BaseConfigurationProvider<MongoTenantedConfiguration>
    {
        public FilterDefinition<Setting> Filter
        {
            get
            {
                var builder = Builders<Setting>.Filter;
                return builder.Regex("Environment", new BsonRegularExpression(Environment, "i")) & builder.Regex("Tenant", new BsonRegularExpression(Configuration.Tenant, "i"));
            }
        }

        public MongoTenantedConfigurationProvider(WebHostBuilderContext context, MongoTenantedConfiguration configuration, IParser<string> parser, ISettingsRepository repository) : base(context, configuration, parser, repository)
        {
            if (string.IsNullOrWhiteSpace(configuration.Tenant))
            {
                throw new ArgumentNullException(nameof(configuration), "Tenant is missing from configuration");
            }
        }

        public override void Load()
        {
            LoadData(Filter);
        }
    }
}