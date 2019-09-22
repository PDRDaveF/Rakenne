using System;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Moq;
using Rakenne.Abstractions.Parsers.Interfaces;
using Rakenne.CosmosDB.Sql.Configurations;
using Rakenne.CosmosDB.Sql.Providers;
using Rakenne.CosmosDB.Sql.WatcherClients.Interfaces;
using Xunit;

namespace Rakenne.CosmosDB.Sql.Tests
{
    public class ProvidersTests
    {
        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenContextMissing()
        {
            // ReSharper disable ObjectCreationAsStatement
            Action sut = () => new CosmosDBConfigurationProvider(null, new CosmosDBConfiguration(), new Mock<IWatcherClient>().Object, new Mock<IParser<string>>().Object);

            sut.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("context");
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenConfigurationMissing()
        {
            // ReSharper disable ObjectCreationAsStatement
            Action sut = () => new CosmosDBConfigurationProvider(new WebHostBuilderContext(), null, new Mock<IWatcherClient>().Object, new Mock<IParser<string>>().Object);

            sut.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("configuration");
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenParserMissing()
        {
            // ReSharper disable ObjectCreationAsStatement
            Action sut = () => new CosmosDBConfigurationProvider(new WebHostBuilderContext(), new CosmosDBConfiguration(), null, new Mock<IParser<string>>().Object);

            sut.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("watcherClient");
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenRepositoryMissing()
        {
            // ReSharper disable ObjectCreationAsStatement
            Action sut = () => new CosmosDBConfigurationProvider(new WebHostBuilderContext(), new CosmosDBConfiguration(), new Mock<IWatcherClient>().Object, null);

            sut.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("parser");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("    ")]
        public void Constructor_ThrowsArgumentNullException_WhenConfigurationMissingConnectionstring(string value)
        {
            var configuration = new CosmosDBConfiguration
            {
                ConnectionString = value
            };

            // ReSharper disable ObjectCreationAsStatement
            Action sut = () => new CosmosDBConfigurationProvider(new WebHostBuilderContext(), configuration, new Mock<IWatcherClient>().Object, new Mock<IParser<string>>().Object);

            sut.Should().ThrowExactly<ArgumentNullException>().And.Message.Should().StartWith("Connectionstring");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("    ")]
        public void Constructor_ThrowsArgumentNullException_WhenConfigurationMissingDatabase(string value)
        {
            var configuration = new CosmosDBConfiguration
            {
                ConnectionString = "A",
                Database = value
            };

            // ReSharper disable ObjectCreationAsStatement
            Action sut = () => new CosmosDBConfigurationProvider(new WebHostBuilderContext(), configuration, new Mock<IWatcherClient>().Object, new Mock<IParser<string>>().Object);

            sut.Should().ThrowExactly<ArgumentNullException>().And.Message.Should().StartWith("Database");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("    ")]
        public void Constructor_ThrowsArgumentNullException_WhenConfigurationMissingPrimaryKey(string value)
        {
            var configuration = new CosmosDBConfiguration
            {
                ConnectionString = "A",
                Database = "B",
                PrimaryKey = value
            };

            // ReSharper disable ObjectCreationAsStatement
            Action sut = () => new CosmosDBConfigurationProvider(new WebHostBuilderContext(), configuration, new Mock<IWatcherClient>().Object, new Mock<IParser<string>>().Object);

            sut.Should().ThrowExactly<ArgumentNullException>().And.Message.Should().StartWith("Primary");
        }

        [Fact]
        public void Constructor_DoesNotThrowArgumentNullException_WhenParametersArePopulated()
        {
            var configuration = new CosmosDBConfiguration
            {
                ConnectionString = "A",
                Database = "B",
                PrimaryKey = "C"
            };

            // ReSharper disable ObjectCreationAsStatement
            Action sut = () => new CosmosDBConfigurationProvider(new WebHostBuilderContext(), configuration, new Mock<IWatcherClient>().Object, new Mock<IParser<string>>().Object);

            sut.Should().NotThrow<ArgumentNullException>();
        }
    }
}