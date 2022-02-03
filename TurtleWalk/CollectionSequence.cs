
using System.Collections;
using TurtleWalks;

public class CollectionSequence : IWalkSequence
{
    public ICollection<(float, Turtle.PenState)> sequence;

    public CollectionSequence(ICollection<(float, Turtle.PenState)> sequence)
        => this.sequence = sequence;

    public int Count => sequence.Count;

    public IEnumerator<(float value, Turtle.PenState penState)> GetEnumerator()
    {
        return sequence.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return sequence.GetEnumerator();
    }
}
