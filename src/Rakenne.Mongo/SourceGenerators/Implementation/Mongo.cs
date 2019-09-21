using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Rakenne.Abstractions.SourceGenerators;
using Rakenne.Abstractions.SourceGenerators.Interfaces;
using Rakenne.Mongo.Abstractions.Configurations;
using Rakenne.Mongo.Abstractions.Repository.Implementation;
using Rakenne.Mongo.Sources;

namespace Rakenne.Mongo.SourceGenerators.Implementation
{
    [ExcludeFromCodeCoverage]
    public sealed class Mongo : ConfigurationSourceGeneratorBase<Mongo>, IConfigurationSourceGenerator
    {
        public IConfigurationSource Create(IConfigurationBuilder configurationBuilder, WebHostBuilderContext context)
        {
            return new MongoConfigurationSource(context, GetConfiguration<MongoConfiguration>(configurationBuilder), new SettingsRepository());
        }
    }
}