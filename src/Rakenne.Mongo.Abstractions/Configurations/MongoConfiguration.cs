using System.Diagnostics.CodeAnalysis;
using Rakenne.Abstractions.Configurations;

namespace Rakenne.Mongo.Abstractions.Configurations
{
    [ExcludeFromCodeCoverage]
    public class MongoConfiguration : DataSourceConfiguration
    {
        public string Database { get; set; }
    }
}