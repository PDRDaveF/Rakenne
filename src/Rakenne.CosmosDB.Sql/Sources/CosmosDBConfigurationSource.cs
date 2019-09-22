using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Rakenne.Abstractions.Parsers.Implementation;
using Rakenne.CosmosDB.Sql.Configurations;
using Rakenne.CosmosDB.Sql.Providers;

namespace Rakenne.CosmosDB.Sql.Sources
{
    public class CosmosDBConfigurationSource : IConfigurationSource
    {
        private readonly WebHostBuilderContext _context;
        private readonly CosmosDBConfiguration _configuration;

        public CosmosDBConfigurationSource(WebHostBuilderContext context, CosmosDBConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new CosmosDBConfigurationProvider(_context, _configuration, new CosmosDBWatcherClient(_context, _configuration), new JsonParser());
        }
    }
}