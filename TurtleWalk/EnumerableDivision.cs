using TurtleWalks;
using System.Collections;
using System.Runtime.CompilerServices;

public class EnumerableDivision : IWalkSequence
{
    public int Divident { get; init; }
    public int Divisor { get; init; }

    int curDivident;
    int remainder = -1;

    public int Count { get; init; }

    public IEnumerator<(float value, Turtle.PenState penState)> GetEnumerator()
    {
        int i = 0;
        curDivident = Divident;
        while (i < Count && remainder != 0) 
        {
            i++;
            var res = curDivident / Divisor;
            remainder = curDivident % Divisor;
            curDivident = remainder * 10;

            yield return (res,Turtle.PenState.Down);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool AlmostEquals(float value1, float value2, float epsilon = 0.0000001f)
    {
        return Math.Abs(value1 - value2) < epsilon;
    }

}
