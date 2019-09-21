using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
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
            _context = context;
            _parser = parser;
            Configuration = configuration;

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
            try
            {
                var database = _client.GetDatabase(Configuration.Database);
                var collection = database.GetCollection<Setting>(_context.HostingEnvironment.ApplicationName);
                var results = collection.Find(filter);

                if (!results.Any())
                {
                    return;
                }

                var settings = results.FirstOrDefault();

                Data = _parser.Parse(settings.Settings);
            }
            catch (Exception exception)
            {
                Data = new Dictionary<string, string> { { "JsonParsingError", exception.Message } };
            }
        }
    }
}