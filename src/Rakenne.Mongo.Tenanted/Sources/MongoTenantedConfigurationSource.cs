using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Rakenne.Abstractions.Parsers.Implementation;
using Rakenne.Mongo.Abstractions.Repository.Interfaces;
using Rakenne.Mongo.Tenanted.Configurations;
using Rakenne.Mongo.Tenanted.Providers.Implementation;

namespace Rakenne.Mongo.Tenanted.Sources
{
    public sealed class MongoTenantedConfigurationSource : IConfigurationSource
    {
        private readonly WebHostBuilderContext _context;
        private readonly MongoTenantedConfiguration _configuration;
        private readonly ISettingsRepository _repository;

        public MongoTenantedConfigurationSource(WebHostBuilderContext context, MongoTenantedConfiguration configuration, ISettingsRepository repository)
        {
            _context = context;
            _configuration = configuration;
            _repository = repository;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new MongoTenantedConfigurationProvider(_context, _configuration, new JsonParser(), _repository);
        }
    }
}