using System;
using System.Collections.Generic;
using System.Threading;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using MongoDB.Driver;
using Moq;
using Rakenne.Abstractions.Models;
using Rakenne.Abstractions.Parsers.Interfaces;
using Rakenne.Mongo.Abstractions.Configurations;
using Rakenne.Mongo.Abstractions.Repository.Interfaces;
using Rakenne.Mongo.Providers.Implementation;
using Xunit;

namespace Rakenne.Mongo.Tests
{
    public class ProvidersTests
    {
        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenContextMissing()
        {
            // ReSharper disable ObjectCreationAsStatement
            Action sut = () => new MongoConfigurationProvider(null, new MongoConfiguration(), new Mock<IParser<string>>().Object, new Mock<ISettingsRepository>().Object);

            sut.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("context");
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenConfigurationMissing()
        {
            // ReSharper disable ObjectCreationAsStatement
            Action sut = () => new MongoConfigurationProvider(new WebHostBuilderContext(), null, new Mock<IParser<string>>().Object, new Mock<ISettingsRepository>().Object);

            sut.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("configuration");
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenParserMissing()
        {
            // ReSharper disable ObjectCreationAsStatement
            Action sut = () => new MongoConfigurationProvider(new WebHostBuilderContext(), new MongoConfiguration(), null, new Mock<ISettingsRepository>().Object);

            sut.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("parser");
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenRepositoryMissing()
        {
            // ReSharper disable ObjectCreationAsStatement
            Action sut = () => new MongoConfigurationProvider(new WebHostBuilderContext(), new MongoConfiguration(), new Mock<IParser<string>>().Object, null);

            sut.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("repository");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("    ")]
        public void Constructor_ThrowsArgumentNullException_WhenConfigurationMissingConnectionstring(string value)
        {
            var configuration = new MongoConfiguration
            {
                ConnectionString = value
            };

            // ReSharper disable ObjectCreationAsStatement
            Action sut = () => new MongoConfigurationProvider(new WebHostBuilderContext(), configuration, new Mock<IParser<string>>().Object, new Mock<ISettingsRepository>().Object);

            sut.Should().ThrowExactly<ArgumentNullException>().And.Message.Should().StartWith("Connectionstring");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("    ")]
        public void Constructor_ThrowsArgumentNullException_WhenConfigurationMissingDatabase(string value)
        {
            var configuration = new MongoConfiguration
            {
                ConnectionString = "A",
                Database = value
            };

            // ReSharper disable ObjectCreationAsStatement
            Action sut = () => new MongoConfigurationProvider(new WebHostBuilderContext(), configuration, new Mock<IParser<string>>().Object, new Mock<ISettingsRepository>().Object);

            sut.Should().ThrowExactly<ArgumentNullException>().And.Message.Should().StartWith("Database");
        }

        [Fact]
        public void Constructor_DoesNotThrowArgumentNullException_WhenParametersArePopulated()
        {
            var configuration = new MongoConfiguration
            {
                ConnectionString = "A",
                Database = "B"
            };

            // ReSharper disable ObjectCreationAsStatement
            Action sut = () => new MongoConfigurationProvider(new WebHostBuilderContext(), configuration, new Mock<IParser<string>>().Object, new Mock<ISettingsRepository>().Object);

            sut.Should().NotThrow<ArgumentNullException>();
        }

        [Fact]
        public void Load_DoesNotThrowException_WhenParametersArePopulated()
        {
            var configuration = new MongoConfiguration
            {
                ConnectionString = "A",
                Database = "B"
            };

            var environment = new Mock<IHostingEnvironment>();

            environment.Setup(item => item.ApplicationName).Returns("C");
            environment.Setup(item => item.EnvironmentName).Returns("D");

            var context = new WebHostBuilderContext
            {
                HostingEnvironment = environment.Object
            };

            var repository = new Mock<ISettingsRepository>();
            var client = new Mock<IMongoClient>();
            var database = new Mock<IMongoDatabase>();
            var collection = new Mock<IMongoCollection<Setting>>();

            client.Setup(item => item.GetDatabase(It.IsAny<string>(), null)).Returns(database.Object);
            database.Setup(item => item.GetCollection<Setting>(It.IsAny<string>(), null)).Returns(collection.Object);
            collection.Setup(item => item.FindSync<Setting>(It.IsAny<FilterDefinition<Setting>>(), null, CancellationToken.None)).Returns((IAsyncCursor<Setting>) null);

            repository.Setup(item => item.CreateClient(It.IsAny<string>())).Returns(client.Object);

            var source = new MongoConfigurationProvider(context, configuration, new Mock<IParser<string>>().Object, repository.Object);
            Action sut = () => source.Load();

            sut.Should().NotThrow<Exception>();

        }

        [Fact]
        public void Load_DoesNotPopulateData_WhenNoSettingsAreFound()
        {
            var configuration = new MongoConfiguration
            {
                ConnectionString = "A",
                Database = "B"
            };

            var environment = new Mock<IHostingEnvironment>();

            environment.Setup(item => item.ApplicationName).Returns("C");
            environment.Setup(item => item.EnvironmentName).Returns("D");

            var context = new WebHostBuilderContext
            {
                HostingEnvironment = environment.Object
            };

            var repository = new Mock<ISettingsRepository>();
            var client = new Mock<IMongoClient>();
            var database = new Mock<IMongoDatabase>();
            var collection = new Mock<IMongoCollection<Setting>>();
            var parser = new Mock<IParser<string>>();

            client.Setup(item => item.GetDatabase(It.IsAny<string>(), null)).Returns(database.Object);
            database.Setup(item => item.GetCollection<Setting>(It.IsAny<string>(), null)).Returns(collection.Object);
            collection.Setup(item => item.FindSync<Setting>(It.IsAny<FilterDefinition<Setting>>(), null, CancellationToken.None)).Returns((IAsyncCursor<Setting>)null);

            repository.Setup(item => item.CreateClient(It.IsAny<string>())).Returns(client.Object);

            var sut = new MongoConfigurationProvider(context, configuration, parser.Object, repository.Object);
            sut.Load();

            parser.Verify(item => item.Parse(It.IsAny<string>()), Times.Never);

        }

        [Fact]
        public void Load_PopulatesData_WhenSettingsAreFound()
        {
            var configuration = new MongoConfiguration
            {
                ConnectionString = "A",
                Database = "B"
            };

            var environment = new Mock<IHostingEnvironment>();

            environment.Setup(item => item.ApplicationName).Returns("C");
            environment.Setup(item => item.EnvironmentName).Returns("D");

            var context = new WebHostBuilderContext
            {
                HostingEnvironment = environment.Object
            };

            var repository = new Mock<ISettingsRepository>();
            var client = new Mock<IMongoClient>();
            var database = new Mock<IMongoDatabase>();
            var collection = new Mock<IMongoCollection<Setting>>();
            var parser = new Mock<IParser<string>>();
            var result = new Mock<IAsyncCursor<Setting>>();

            client.Setup(item => item.GetDatabase(It.IsAny<string>(), null)).Returns(database.Object);
            database.Setup(item => item.GetCollection<Setting>(It.IsAny<string>(), null)).Returns(collection.Object);
            collection.Setup(item => item.FindSync<Setting>(It.IsAny<FilterDefinition<Setting>>(), null, CancellationToken.None)).Returns(result.Object);
            result.Setup(item => item.Current).Returns(new List<Setting> {new Setting{ Settings = "{\"E\":\"F\"}"}});

            repository.Setup(item => item.CreateClient(It.IsAny<string>())).Returns(client.Object);

            var sut = new MongoConfigurationProvider(context, configuration, parser.Object, repository.Object);
            sut.Load();

            parser.Verify(item => item.Parse(It.IsAny<string>()), Times.Once);

        }
    }
}