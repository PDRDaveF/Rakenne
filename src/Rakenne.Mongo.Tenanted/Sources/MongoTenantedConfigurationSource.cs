using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Rakenne.Abstractions.Parsers.Implementation;
using Rakenne.Mongo.Tenanted.Configurations;
using Rakenne.Mongo.Tenanted.Providers;

namespace Rakenne.Mongo.Tenanted.Sources
{
    public class MongoTenantedConfigurationSource : IConfigurationSource
    {
        private readonly WebHostBuilderContext _context;
        private readonly MongoTenantedConfiguration _configuration;

        public MongoTenantedConfigurationSource(WebHostBuilderContext context, MongoTenantedConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new MongoTenantedConfigurationProvider(_context, _configuration, new JsonParser());
        }
    }
}