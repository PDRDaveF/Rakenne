using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Rakenne.Abstractions.SourceGenerators.Interfaces;

namespace Rakenne.Abstractions
{
    [ExcludeFromCodeCoverage]
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddDataSource<T>(this IConfigurationBuilder configurationBuilder, WebHostBuilderContext context)
        where T : IConfigurationSourceGenerator, new()
        {
            return configurationBuilder.Add(new T().Create(configurationBuilder, context));
        }
    }
}
