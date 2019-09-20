using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Rakenne.Abstractions.Parsers.Implementation;
using Rakenne.Mongo.Configurations;
using Rakenne.Mongo.Providers;

namespace Rakenne.Mongo.Sources
{
    public class MongoConfigurationSource : IConfigurationSource
    {
        private readonly WebHostBuilderContext _context;
        private readonly MongoConfiguration _configuration;

        public MongoConfigurationSource(WebHostBuilderContext context, MongoConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new MongoConfigurationProvider(_context, _configuration, new JsonParser());
        }
    }
}