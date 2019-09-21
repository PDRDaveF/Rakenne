using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Rakenne.Abstractions.Parsers.Implementation;
using Rakenne.Mongo.Abstractions.Configurations;
using Rakenne.Mongo.Abstractions.Repository.Interfaces;
using Rakenne.Mongo.Providers.Implementation;

namespace Rakenne.Mongo.Sources
{
    public sealed class MongoConfigurationSource : IConfigurationSource
    {
        private readonly WebHostBuilderContext _context;
        private readonly MongoConfiguration _configuration;
        private readonly ISettingsRepository _repository;

        public MongoConfigurationSource(WebHostBuilderContext context, MongoConfiguration configuration, ISettingsRepository repository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new MongoConfigurationProvider(_context, _configuration, new JsonParser(), _repository);
        }
    }
}