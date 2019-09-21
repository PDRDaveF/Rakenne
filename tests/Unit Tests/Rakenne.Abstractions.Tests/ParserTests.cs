using System.Linq;
using FluentAssertions;
using Rakenne.Abstractions.Parsers.Implementation;
using Xunit;

namespace Rakenne.Abstractions.Tests
{
    public class ParserTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("    ")]
        [InlineData("A")]
        [InlineData("\"A\":\"B\"")]
        public void Parse_ReturnsEmptyDictionary_WhenJsonIsInvalid(string json)
        {
            var parse = new JsonParser();
            var sut = parse.Parse(json);

            sut.Any().Should().BeFalse();
        }

        [Theory]
        [InlineData("{\"A\":\"B\"}", 1)]
        [InlineData("{\"A\":{\"C\":\"D\"}}", 1)]
        [InlineData("{\"A\":[1,2,3]}", 3)]
        public void Parser_ReturnsDictionary_WhenJsonIsValid(string json, int expectedNumberOfKeys)
        {
            var parse = new JsonParser();
            var sut = parse.Parse(json);

            sut.Any().Should().BeTrue();
            sut.Count.Should().Be(expectedNumberOfKeys);
        }
    }
}
