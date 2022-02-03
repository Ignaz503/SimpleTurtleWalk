using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;


namespace TurtleWalks;

public class Turtle
{
    public enum PenState 
    {
        Up,
        Down
    }

    public Turtle.PenState Pen { get; set; } = Turtle.PenState.Down;
    public Vector2 Position { get; private set; }
    Vector2 lookAt;

    public Turtle(Vector2 position, Vector2 lookAt)
        => (this.Position, this.lookAt) =(position, lookAt);

    public Turtle(Vector2 position, Vector2 lookAt, Turtle.PenState penState)
    => (this.Position, this.lookAt, this.Pen) = (position, lookAt, penState);


    public void Forward(float units) 
    {
        Position += lookAt * units;
    }

    public void Rotate(Radians r)
        => lookAt = Vector2.Normalize(new Vector2(
            MathF.Cos(r)*lookAt.X - MathF.Sin(r)*lookAt.Y,
            MathF.Sin(r)*lookAt.X + MathF.Cos(r)*lookAt.Y));

    public void Rotate(Degree deg)
        => Rotate((Radians)deg);

}
