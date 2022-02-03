namespace TurtleWalks;

public struct Radians 
{
    public const float RadToDeg = 180.0f / MathF.PI;
    float val;

    public Radians(float val)
        => this.val = val;


    public static implicit operator float(Radians r)
        => r.val;

    public static explicit operator Radians(float val)
        => new Radians(val); 
    public static explicit operator Degree(Radians r)
        => new Degree(r.val*RadToDeg);

    public override string ToString()
    {
        
        return $"{val}r";
    }
}

