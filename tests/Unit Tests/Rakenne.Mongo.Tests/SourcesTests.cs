using System;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Moq;
using Rakenne.Mongo.Abstractions.Configurations;
using Rakenne.Mongo.Abstractions.Repository.Interfaces;
using Rakenne.Mongo.Sources;
using Xunit;

namespace Rakenne.Mongo.Tests
{
    public class SourcesTests
    {
        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenContextMissing()
        {
            // ReSharper disable ObjectCreationAsStatement
            Action sut = () => new MongoConfigurationSource(null, null, null);

            sut.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("context");
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenConfigurationMissing()
        {
            // ReSharper disable ObjectCreationAsStatement
            Action sut = () => new MongoConfigurationSource(new WebHostBuilderContext(), null, null);

            sut.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("configuration");
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenRepositoryMissing()
        {
            // ReSharper disable ObjectCreationAsStatement
            Action sut = () => new MongoConfigurationSource(new WebHostBuilderContext(), new MongoConfiguration(), null);

            sut.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("repository");
        }

        [Fact]
        public void Constructor_DoesNotThrowArgumentNullException_WhenParametersArePopulated()
        {
            // ReSharper disable ObjectCreationAsStatement
            Action sut = () => new MongoConfigurationSource(new WebHostBuilderContext(), new MongoConfiguration(), new Mock<ISettingsRepository>().Object);

            sut.Should().NotThrow<ArgumentNullException>();
        }

        [Fact]
        public void Build_DoesNotThrowException_WhenParametersArePopulated()
        {
            var configuration = new MongoConfiguration
            {
                ConnectionString = "A",
                Database = "B"
            };

            var source = new MongoConfigurationSource(new WebHostBuilderContext(), configuration, new Mock<ISettingsRepository>().Object);
            Action sut = () => source.Build(new ConfigurationBuilder());

            sut.Should().NotThrow<Exception>();

        }
    }
}