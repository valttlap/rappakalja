namespace Sanasoppa.API.Extensions;

public static class EnumerableExtensions
{
    private static readonly Random random = new Random();

    public static IEnumerable<T> Scramble<T>(this IEnumerable<T> source)
    {
        return source.OrderBy(x => random.Next());
    }
}
