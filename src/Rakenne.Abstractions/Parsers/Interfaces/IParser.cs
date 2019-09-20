using System.Collections.Generic;

namespace Rakenne.Abstractions.Parsers.Interfaces
{
    public interface IParser<in T>
    {
        IDictionary<string, string> Parse(T json);
    }
}