using System;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Cosmos;
using Moq;
using Rakenne.CosmosDB.Sql.Configurations;
using Rakenne.CosmosDB.Sql.WatcherClients.Implementation;
using Xunit;

namespace Rakenne.CosmosDB.Sql.Tests
{
    public class WatcherTests
    {
        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenClientMissing()
        {
            // ReSharper disable ObjectCreationAsStatement
            Action sut = () => new CosmosDBWatcherClient(null, new WebHostBuilderContext(), new CosmosDBConfiguration());

            sut.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("cosmosClient");
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenContextMissing()
        {
            // ReSharper disable ObjectCreationAsStatement
            Action sut = () => new CosmosDBWatcherClient(new Mock<CosmosClient>().Object, null, new CosmosDBConfiguration());

            sut.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("context");
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenConfigrationMissing()
        {
            // ReSharper disable ObjectCreationAsStatement
            Action sut = () => new CosmosDBWatcherClient(new Mock<CosmosClient>().Object, new WebHostBuilderContext(), null);

            sut.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("configuration");
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenConfigrationMissingDatabase()
        {
            // ReSharper disable ObjectCreationAsStatement
            Action sut = () => new CosmosDBWatcherClient(new Mock<CosmosClient>().Object, new WebHostBuilderContext(), new CosmosDBConfiguration());

            sut.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("configuration");
            sut.Should().ThrowExactly<ArgumentNullException>().And.Message.Should().StartWith("Database");
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenConfigurationMissingLeaseContainerName()
        {
            // ReSharper disable ObjectCreationAsStatement
            Action sut = () => new CosmosDBWatcherClient(new Mock<CosmosClient>().Object, new WebHostBuilderContext(), new CosmosDBConfiguration{Database = "A"});

            sut.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("configuration");
            sut.Should().ThrowExactly<ArgumentNullException>().And.Message.Should().StartWith("Lease");
        }
    }
}