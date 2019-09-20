using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using Rakenne.Abstractions.Models;
using Rakenne.Abstractions.Parsers.Interfaces;
using Rakenne.Mongo.Tenanted.Configurations;

namespace Rakenne.Mongo.Tenanted.Providers
{
    public class MongoTenantedConfigurationProvider : ConfigurationProvider
    {
        private readonly WebHostBuilderContext _context;
        private readonly MongoTenantedConfiguration _configuration;
        private readonly MongoClient _client;
        private readonly IParser<string> _parser;

        public MongoTenantedConfigurationProvider(WebHostBuilderContext context, MongoTenantedConfiguration configuration, IParser<string> parser)
        {
            _context = context;
            _parser = parser;
            _configuration = configuration;

            if (string.IsNullOrWhiteSpace(configuration.ConnectionString))
            {
                throw new ArgumentNullException(nameof(configuration), "Connectionstring is missing from configuration");
            }

            if (string.IsNullOrWhiteSpace(configuration.Database))
            {
                throw new ArgumentNullException(nameof(configuration), "Database is missing from configuration");
            }

            if (string.IsNullOrWhiteSpace(configuration.Tenant))
            {
                throw new ArgumentNullException(nameof(configuration), "Tenant is missing from configuration");
            }

            var settings = MongoClientSettings.FromUrl(new MongoUrl(configuration.ConnectionString));
            _client = new MongoClient(settings);
        }

        public override void Load()
        {
            try
            {
                var database = _client.GetDatabase(_configuration.Database);
                var collection = database.GetCollection<Setting>(_context.HostingEnvironment.ApplicationName);
                var builder = Builders<Setting>.Filter;
                var filter = builder.Regex("Environment", new BsonRegularExpression(_context.HostingEnvironment.EnvironmentName, "i")) & builder.Regex("Tenant", new BsonRegularExpression(_configuration.Tenant, "i"));
                
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
                Data = new Dictionary<string, string>{{ "JsonParsingError", exception.Message } };
            }
        }

        
    }
}