using System.Diagnostics.CodeAnalysis;

namespace Rakenne.Abstractions.Configurations
{
    [ExcludeFromCodeCoverage]
    public class DataSourceConfiguration
    {
        public string ConnectionString { get; set; }

        public string Environment { get; set; }

        public string DataSourceName { get; set; }
    }
}