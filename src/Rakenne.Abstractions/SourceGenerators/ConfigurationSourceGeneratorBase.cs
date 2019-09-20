using Microsoft.Extensions.Configuration;
using Rakenne.Abstractions.SourceGenerators.Interfaces;

namespace Rakenne.Abstractions.SourceGenerators
{
    public abstract class ConfigurationSourceGeneratorBase<T> where T : IConfigurationSourceGenerator
    {
        public string Key => $"configuration:{typeof(T).Name.ToLowerInvariant()}";

        protected TConfiguration GetConfiguration<TConfiguration>(IConfigurationBuilder configurationBuilder) where TConfiguration : new()
        {
            var configurationRoot = configurationBuilder.Build();
            var configuration = new TConfiguration();

            configurationRoot.Bind(Key, configuration);

            return configuration;
        }
    }
}
