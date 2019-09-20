using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Rakenne.Abstractions.SourceGenerators.Interfaces
{
    public interface IConfigurationSourceGenerator
    {
        string Key { get; }

        IConfigurationSource Create(IConfigurationBuilder configurationBuilder, WebHostBuilderContext context);
    }
}