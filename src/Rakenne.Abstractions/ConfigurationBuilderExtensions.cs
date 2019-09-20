using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Rakenne.Abstractions.Configurations;
using Rakenne.Abstractions.SourceGenerators.Interfaces;

namespace Rakenne.Abstractions
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddDataSource<T>(this IConfigurationBuilder configurationBuilder, WebHostBuilderContext context)
        where T : IConfigurationSourceGenerator, new()
        {
            return configurationBuilder.Add(new T().Create(configurationBuilder, context));
        }

        public static IConfigurationBuilder AddDataSource<T, TSettings>(this IConfigurationBuilder configurationBuilder, WebHostBuilderContext context, TSettings settings)
            where T : IConfigurationSourceGenerator, new()
            where TSettings : DataSourceConfiguration
        {
            return configurationBuilder.Add(new T().Create(configurationBuilder, context));
        }
    }
}
