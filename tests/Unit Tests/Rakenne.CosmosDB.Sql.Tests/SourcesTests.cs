using System;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Cosmos;
using Moq;
using Rakenne.CosmosDB.Sql.Configurations;
using Rakenne.CosmosDB.Sql.Sources;
using Xunit;

namespace Rakenne.CosmosDB.Sql.Tests
{
    public class SourcesTests
    {
        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenContextMissing()
        {
            // ReSharper disable ObjectCreationAsStatement
            Action sut = () => new CosmosDBConfigurationSource(null, null, null);

            sut.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("context");
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenConfigurationMissing()
        {
            // ReSharper disable ObjectCreationAsStatement
            Action sut = () => new CosmosDBConfigurationSource(new WebHostBuilderContext(), null, null);

            sut.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("configuration");
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenClientMissing()
        {
            // ReSharper disable ObjectCreationAsStatement
            Action sut = () => new CosmosDBConfigurationSource(new WebHostBuilderContext(), new CosmosDBConfiguration(), null);

            sut.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("client");
        }

        [Fact]
        public void Constructor_DoesNotThrowArgumentNullException_WhenParametersArePopulated()
        {
            // ReSharper disable ObjectCreationAsStatement
            Action sut = () => new CosmosDBConfigurationSource(new WebHostBuilderContext(), new CosmosDBConfiguration{ Database = "A", LeaseContainerName = "B" }, new Mock<CosmosClient>().Object);

            sut.Should().NotThrow<ArgumentNullException>();
        }
    }
}