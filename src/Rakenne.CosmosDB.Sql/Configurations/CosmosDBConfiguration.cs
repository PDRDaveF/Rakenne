﻿using System.Diagnostics.CodeAnalysis;
using Rakenne.Abstractions.Configurations;

namespace Rakenne.CosmosDB.Sql.Configurations
{
    [ExcludeFromCodeCoverage]
    public class CosmosDBConfiguration : DataSourceConfiguration
    {
        public string Database { get; set; }

        public string PrimaryKey { get; set; }

        public string LeaseContainerName { get; set; }

        public bool ReloadOnChange { get; set; }

        public int PollingIntervalInSeconds { get; set; }
    }
}