using System.Numerics;

namespace TurtleWalks;

public class TurtleWalk 
{
    public float StepSize { get; set; }

    Turtle turtle;

    public TurtleWalk()
    {
        StepSize = 1;
        turtle = new(Vector2.Zero, Vector2.UnitX);
    }

    public TurtleWalk(Vector2 initalPosiotn, Vector2 initialLookAt, float setpSize = 1)
        => (this.StepSize, this.turtle) = (setpSize, new(initalPosiotn, initialLookAt));

    public TurtleWalk(Vector2 initalPosiotn, Radians initialRotation, float setpSize = 1)
        => (this.StepSize, this.turtle) = (setpSize, new(initalPosiotn, new(MathF.Cos(initialRotation),MathF.Sin(initialRotation))));



    public TurtleWalkResult Walk(WalkInfo info, Action<ulong> lifeSignal) 
    {
        TurtleWalkResult result = new(info.WalkSequence.Count);
        result.Add(turtle.Position);

        Walk(info, (u, step) =>
        {
            lifeSignal(u);
            result.Add(step.CurrentPosition);
        });
        return result;
    }

    public void Walk(WalkInfo info, Action<ulong,Step> report)
    {
        var rotStepSize = (360f / info.Base);
        ulong i = 0;

        foreach (var (val, pen) in info.WalkSequence)
        {

            i++;
            var rot = new Degree(rotStepSize * val);
            rot.Clamp0To360();


            var old = turtle.Position;

            turtle.Pen = pen;
            turtle.Rotate(rot);
            turtle.Forward(StepSize);


            if (turtle.Pen == Turtle.PenState.Up)
                continue;

            report(i, new Step() { PreviousPosition = old, CurrentPosition = turtle.Position });
        }
    }

    public struct Step 
    {
        public Vector2 PreviousPosition { get; init; }
        public Vector2 CurrentPosition { get; init; }
    }

}

