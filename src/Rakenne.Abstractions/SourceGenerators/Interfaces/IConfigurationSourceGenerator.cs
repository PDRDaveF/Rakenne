using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Rakenne.Abstractions.SourceGenerators.Interfaces
{
    public interface IConfigurationSourceGenerator
    {
        IConfigurationSource Create(IConfigurationBuilder configurationBuilder, WebHostBuilderContext context);
    }
}