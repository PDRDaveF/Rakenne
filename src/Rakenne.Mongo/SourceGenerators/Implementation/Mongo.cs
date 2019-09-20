using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Rakenne.Abstractions.SourceGenerators;
using Rakenne.Abstractions.SourceGenerators.Interfaces;
using Rakenne.Mongo.Configurations;
using Rakenne.Mongo.Sources;

namespace Rakenne.Mongo.SourceGenerators.Implementation
{
    public class Mongo : ConfigurationSourceGeneratorBase<Mongo>, IConfigurationSourceGenerator
    {
        public IConfigurationSource Create(IConfigurationBuilder configurationBuilder, WebHostBuilderContext context)
        {
            return new MongoConfigurationSource(context, GetConfiguration<MongoConfiguration>(configurationBuilder));
        }
    }
}