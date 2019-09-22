using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Rakenne.Abstractions.Models;
using Rakenne.Abstractions.Parsers.Interfaces;
using Rakenne.CosmosDB.Sql.Configurations;
using Rakenne.CosmosDB.Sql.WatcherClients.Interfaces;

namespace Rakenne.CosmosDB.Sql.Providers
{
    public class CosmosDBConfigurationProvider : ConfigurationProvider, IDisposable
    {
        private readonly WebHostBuilderContext _context;
        private readonly CosmosDBConfiguration _configuration;
        private readonly IDisposable _changeTokenRegistration;
        private readonly IParser<string> _parser;

        private const string PartitionKey = "environment";

        private static FeedOptions FeedOptions => new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true };

        public CosmosDBConfigurationProvider(WebHostBuilderContext context, CosmosDBConfiguration configuration, IWatcherClient watcherClient, IParser<string> parser)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));

            if (watcherClient == null)
            {
                throw new ArgumentNullException(nameof(watcherClient));
            }

            if (string.IsNullOrWhiteSpace(configuration.ConnectionString))
            {
                throw new ArgumentNullException(nameof(configuration), "Connectionstring is missing from configuration");
            }

            if (string.IsNullOrWhiteSpace(configuration.Database))
            {
                throw new ArgumentNullException(nameof(configuration), "Database is missing from configuration");
            }

            if (string.IsNullOrWhiteSpace(configuration.PrimaryKey))
            {
                throw new ArgumentNullException(nameof(configuration), "Primary key is missing from configuration");
            }

            if (_configuration.ReloadOnChange)
            {
                _changeTokenRegistration = ChangeToken.OnChange(watcherClient.Watch, LoadSettings);
            }
        }

        private void LoadSettings()
        {
            var settings = Search();

            if(settings == null)
            {
                return;
            }

            Data = _parser.Parse(settings.Settings);
        }

        public override void Load()
        {
            LoadSettings();
        }

        private SqlQuerySpec Query()
        {
            return new SqlQuerySpec
            {
                QueryText = $"SELECT c.settings FROM c WHERE c.{PartitionKey} = @{PartitionKey}",
                Parameters = new SqlParameterCollection
                {
                    new SqlParameter($"@{PartitionKey}", _context.HostingEnvironment.EnvironmentName.ToLowerInvariant())
                }
            };
        }

        private Setting Search()
        {
            using (var client = new DocumentClient(new Uri(_configuration.ConnectionString), _configuration.PrimaryKey))
            {
                try
                {
                    return client
                        .CreateDocumentQuery<Setting>(
                            UriFactory.CreateDocumentCollectionUri(_configuration.Database, _context.HostingEnvironment.ApplicationName), Query(),
                            FeedOptions).ToList().FirstOrDefault();
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public void Dispose()
        {
            _changeTokenRegistration?.Dispose();
        }
    }

}