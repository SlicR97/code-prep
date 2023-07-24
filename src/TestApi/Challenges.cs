using System.Collections.Generic;

namespace TestApi;

public record Challenge(object Data, object Solution);

public static class Challenges
{
    public static IDictionary<string, Challenge> Values = new Dictionary<string, Challenge>
    {
        { "01", new Challenge(new List<int> { 1, 2, 3, 4 }, new List<int> { 4, 3, 2, 1 }) }
    };
}