using System.Numerics;


namespace TurtleWalks;

public class TurtleWalkResult : List<Vector2> 
{
    MinTracker xMin,yMin;
    MaxTracker xMax, yMax;

    public Vector2 Min => new(xMin.Val, yMin.Val);
    public Vector2 Max => new(xMax.Val, yMax.Val);


    public TurtleWalkResult(int capacity): base(capacity)
    {}

    public new void Add(Vector2 item) 
    {
        base.Add(item);
        xMin.Update(item.X);
        xMax.Update(item.X);
        yMin.Update(item.Y);
        yMax.Update(item.Y);
    }

}
