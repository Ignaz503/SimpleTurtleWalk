namespace TurtleWalks;

public interface IWalkSequence : IEnumerable<(float value, Turtle.PenState penState)>
{
    public int Count { get;  }
}
