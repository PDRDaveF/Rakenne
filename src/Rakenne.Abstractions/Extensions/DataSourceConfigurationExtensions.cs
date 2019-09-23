using Microsoft.AspNetCore.Hosting;
using Rakenne.Abstractions.Configurations;

namespace Rakenne.Abstractions.Extensions
{
    public static class DataSourceConfigurationExtensions
    {
        public static string GetEnvironment(this DataSourceConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            return string.IsNullOrWhiteSpace(configuration.Environment)
                ? hostingEnvironment.EnvironmentName
                : configuration.Environment;
        }

        public static string GetDataSourceName(this DataSourceConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            return string.IsNullOrWhiteSpace(configuration.DataSourceName)
                ? hostingEnvironment.ApplicationName
                : configuration.DataSourceName;
        }
    }
}