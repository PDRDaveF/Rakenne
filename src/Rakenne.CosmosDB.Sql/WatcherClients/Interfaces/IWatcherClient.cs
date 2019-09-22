using Microsoft.Extensions.Primitives;

namespace Rakenne.CosmosDB.Sql.WatcherClients.Interfaces
{
    public interface IWatcherClient
    {
        IChangeToken Watch();
    }
}