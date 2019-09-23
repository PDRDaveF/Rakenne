using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Rakenne.Abstractions.Extensions;
using Rakenne.Abstractions.Models;
using Rakenne.Abstractions.Parsers.Interfaces;
using Rakenne.Mongo.Abstractions.Configurations;
using Rakenne.Mongo.Abstractions.Repository.Interfaces;

namespace Rakenne.Mongo.Abstractions.Providers
{
    public abstract class BaseConfigurationProvider<TConfiguration> : ConfigurationProvider where TConfiguration : MongoConfiguration
    {
        private readonly WebHostBuilderContext _context;
        protected readonly TConfiguration Configuration;
        private readonly IMongoClient _client;
        private readonly IParser<string> _parser;

        protected string Environment => _context.HostingEnvironment.EnvironmentName;

        protected BaseConfigurationProvider(WebHostBuilderContext context, TConfiguration configuration, IParser<string> parser, ISettingsRepository repository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));

            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            if (string.IsNullOrWhiteSpace(configuration.ConnectionString))
            {
                throw new ArgumentNullException(nameof(configuration), "Connectionstring is missing from configuration");
            }

            if (string.IsNullOrWhiteSpace(configuration.Database))
            {
                throw new ArgumentNullException(nameof(configuration), "Database is missing from configuration");
            }

            _client = repository.CreateClient(configuration.ConnectionString);
        }

        protected void LoadData(FilterDefinition<Setting> filter)
        {
            var settings = Search(filter);

            if (settings == null)
            {
                return;
            }

            Data = _parser.Parse(settings.Settings);
        }

        private Setting Search(FilterDefinition<Setting> filter)
        {
            var database = _client.GetDatabase(Configuration.Database);
            var collection = database.GetCollection<Setting>(Configuration.GetDataSourceName(_context.HostingEnvironment));
            var results = collection.FindSync<Setting>(filter)?.Current?.ToList() ?? new List<Setting>();

            return results.Any() ? results.FirstOrDefault() : null;
        }
    }
}