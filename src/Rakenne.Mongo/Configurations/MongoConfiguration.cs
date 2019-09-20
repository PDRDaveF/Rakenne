using Rakenne.Abstractions.Configurations;

namespace Rakenne.Mongo.Configurations
{
    public class MongoConfiguration : DataSourceConfiguration
    {
        public string Database { get; set; }
    }
}