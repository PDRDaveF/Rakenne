using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Rakenne.Abstractions;

namespace Rakenne.TestHarness
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, configuration) =>
                {
                    var config = configuration.Build();
                    var implementation = config["configtype"];

                    switch (implementation)
                    {
                        case "cosmosdb":
                            configuration.AddDataSource<CosmosDB.Sql.SourceGenerators.Implementation.CosmosDB>(context);
                            break;
                        case "mongo":
                            configuration.AddDataSource<Mongo.SourceGenerators.Implementation.Mongo>(context);
                            break;
                        case "mongotenanted":
                            configuration.AddDataSource<Mongo.Tenanted.SourceGenerators.Implementation.MongoTenanted>(context);
                            break;
                    }
                })
                .UseStartup<Startup>();
    }
}
