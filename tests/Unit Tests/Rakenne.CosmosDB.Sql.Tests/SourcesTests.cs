using System;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
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
            Action sut = () => new CosmosDBConfigurationSource(null, null);

            sut.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("context");
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenConfigurationMissing()
        {
            // ReSharper disable ObjectCreationAsStatement
            Action sut = () => new CosmosDBConfigurationSource(new WebHostBuilderContext(), null);

            sut.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("configuration");
        }

        [Fact]
        public void Constructor_DoesNotThrowArgumentNullException_WhenParametersArePopulated()
        {
            // ReSharper disable ObjectCreationAsStatement
            Action sut = () => new CosmosDBConfigurationSource(new WebHostBuilderContext(), new CosmosDBConfiguration());

            sut.Should().NotThrow<ArgumentNullException>();
        }
    }
}