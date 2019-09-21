using Microsoft.AspNetCore.Hosting;
using MongoDB.Bson;
using MongoDB.Driver;
using Rakenne.Abstractions.Models;
using Rakenne.Abstractions.Parsers.Interfaces;
using Rakenne.Mongo.Abstractions.Configurations;
using Rakenne.Mongo.Abstractions.Providers;
using Rakenne.Mongo.Abstractions.Repository.Interfaces;

namespace Rakenne.Mongo.Providers.Implementation
{
    public sealed class MongoConfigurationProvider : BaseConfigurationProvider<MongoConfiguration>
    {
        public FilterDefinition<Setting> Filter => Builders<Setting>.Filter.Regex("Environment", new BsonRegularExpression(Environment, "i"));

        public MongoConfigurationProvider(WebHostBuilderContext context, MongoConfiguration configuration, IParser<string> parser, ISettingsRepository repository) : base(context, configuration, parser, repository)
        {
        }

        public override void Load()
        {
            LoadData(Filter);
        }
    }
}