using Rakenne.Mongo.Abstractions.Configurations;

namespace Rakenne.Mongo.Tenanted.Configurations
{
    public sealed class MongoTenantedConfiguration : MongoConfiguration
    {
        public string Tenant { get; set; }
    }
}