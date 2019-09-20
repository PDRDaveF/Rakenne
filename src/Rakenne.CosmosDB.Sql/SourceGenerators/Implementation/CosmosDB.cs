using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Rakenne.Abstractions.SourceGenerators;
using Rakenne.Abstractions.SourceGenerators.Interfaces;
using Rakenne.CosmosDB.Sql.Configurations;
using Rakenne.CosmosDB.Sql.Sources;

namespace Rakenne.CosmosDB.Sql.SourceGenerators.Implementation
{
    public class CosmosDB : ConfigurationSourceGeneratorBase<CosmosDB>, IConfigurationSourceGenerator
    {
        public IConfigurationSource Create(IConfigurationBuilder configurationBuilder, WebHostBuilderContext context)
        {
            return new CosmosDBConfigurationSource(context, GetConfiguration<CosmosDBConfiguration>(configurationBuilder));
        }
    }
}