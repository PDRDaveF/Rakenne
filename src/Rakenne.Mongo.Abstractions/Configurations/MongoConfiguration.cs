using Rakenne.Abstractions.Configurations;

namespace Rakenne.Mongo.Abstractions.Configurations
{
    public class MongoConfiguration : DataSourceConfiguration
    {
        public string Database { get; set; }
    }
}