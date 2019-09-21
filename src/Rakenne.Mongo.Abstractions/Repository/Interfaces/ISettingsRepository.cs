using MongoDB.Driver;

namespace Rakenne.Mongo.Abstractions.Repository.Interfaces
{
    public interface ISettingsRepository
    {
        IMongoClient CreateClient(string connectionstring);
    }
}