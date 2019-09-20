using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Rakenne.Abstractions.SourceGenerators;
using Rakenne.Abstractions.SourceGenerators.Interfaces;
using Rakenne.Mongo.Tenanted.Configurations;
using Rakenne.Mongo.Tenanted.Sources;

namespace Rakenne.Mongo.Tenanted.SourceGenerators.Implementation
{
    public class MongoTenanted : ConfigurationSourceGeneratorBase<MongoTenanted>, IConfigurationSourceGenerator
    {
        public IConfigurationSource Create(IConfigurationBuilder configurationBuilder, WebHostBuilderContext context)
        {
            return new MongoTenantedConfigurationSource(context, GetConfiguration<MongoTenantedConfiguration>(configurationBuilder));
        }
    }
}