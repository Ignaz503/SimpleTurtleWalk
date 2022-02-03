using System.Runtime.CompilerServices;

namespace TurtleWalks;

public struct Degree 
{
    public const float DegToRad = MathF.PI / 180.0f;
    float val;

    public Degree(float val)
        => this.val = val;

    public static implicit operator float(Degree d)
        => d.val;

    public static explicit operator Degree(float val)
        => new Degree(val);

    public static explicit operator Radians(Degree d)
        => new Radians(d.val*DegToRad);

    public void Clamp0To360() 
    {
        val = ModularClamp(val, 0, 360, 0, 360);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ModularClamp(float val, float min, float max, float rangemin, float rangemax)
    {
        var modulus = Math.Abs(rangemax - rangemin);
        if ((val %= modulus) < 0f)//??
            val += modulus;
        return Math.Clamp(val + Math.Min(rangemin, rangemax), min, max);
    }

    public override string ToString()
    {
        return $"{val}°";
    }

}

