using Rakenne.Mongo.Configurations;

namespace Rakenne.Mongo.Tenanted.Configurations
{
    public class MongoTenantedConfiguration : MongoConfiguration
    {
        public string Tenant { get; set; }
    }
}