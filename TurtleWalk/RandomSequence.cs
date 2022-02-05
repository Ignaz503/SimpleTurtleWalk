using TurtleWalks;
using System.Collections;

public class RandomSequence : IWalkSequence
{
    public int Count { get; init; }
    public int Seed { get; init; }
    public int MaxInclusive { get; init; }
    public int MinInclusive { get; init; }

    public IEnumerator<(float value, Turtle.PenState penState)> GetEnumerator()
    {
        var rng = new System.Random(Seed);
        for (int i = 0; i < Count; i++)
        {
            yield return (rng.Next(MinInclusive, MaxInclusive + 1), Turtle.PenState.Down);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}