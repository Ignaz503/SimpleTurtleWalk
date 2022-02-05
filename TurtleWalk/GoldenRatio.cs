using TurtleWalks;
using System.Collections;

public class GoldenRatio : IWalkSequence
{
    public int Count { get; init; }

    public IEnumerator<(float value, Turtle.PenState penState)> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}