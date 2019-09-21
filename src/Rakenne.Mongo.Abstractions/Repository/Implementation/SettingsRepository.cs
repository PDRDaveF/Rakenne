using System.Diagnostics.CodeAnalysis;
using MongoDB.Driver;
using Rakenne.Mongo.Abstractions.Repository.Interfaces;

namespace Rakenne.Mongo.Abstractions.Repository.Implementation
{
    [ExcludeFromCodeCoverage]
    public class SettingsRepository : ISettingsRepository
    {
        public IMongoClient CreateClient(string connectionString)
        {
            var settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            return new MongoClient(settings);
        }
    }
}