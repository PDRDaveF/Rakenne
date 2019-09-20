using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Rakenne.Abstractions.Models;
using Rakenne.Abstractions.Parsers.Implementation;
using Rakenne.CosmosDB.Sql.Configurations;

namespace Rakenne.CosmosDB.Sql.Providers
{
    public class CosmosDBConfigurationProvider : ConfigurationProvider, IDisposable
    {
        private readonly WebHostBuilderContext _context;
        private readonly CosmosDBConfiguration _configuration;
        private readonly IDisposable _changeTokenRegistration;

        public CosmosDBConfigurationProvider(WebHostBuilderContext context, CosmosDBConfiguration configuration, CosmosDBWatcherClient watcherClient)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            
            if (watcherClient == null)
            {
                throw new ArgumentNullException(nameof(watcherClient));
            }
            
            if (_configuration.ReloadOnChange)
            {
                _changeTokenRegistration = ChangeToken.OnChange(watcherClient.Watch, LoadSettings);
            }
        }

        private void LoadSettings()
        {
            using (var client = new DocumentClient(new Uri(_configuration.ConnectionString), _configuration.PrimaryKey))
            {
                var query = new SqlQuerySpec
                {
                    QueryText = "SELECT c.settings FROM c WHERE c.environment = @environment",
                    Parameters = new SqlParameterCollection
                    {
                        new SqlParameter("@environment", _context.HostingEnvironment.EnvironmentName.ToLowerInvariant())
                    }
                };
                var queryOptions = new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true };
                var configQuery = client.CreateDocumentQuery<Setting>(UriFactory.CreateDocumentCollectionUri(_configuration.Database, "ConfigTest"), query, queryOptions).ToList();

                var settings = configQuery.FirstOrDefault();

                if (settings == null)
                {
                    return;
                }

                var parser = new JsonParser();

                Data = parser.Parse(settings.Settings);
            }
        }

        public override void Load()
        {
            LoadSettings();
        }

        public void Dispose()
        {
            _changeTokenRegistration?.Dispose();
        }
    }

}