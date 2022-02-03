using TurtleWalks;
using System.Numerics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Collections;
using System.Runtime.CompilerServices;


var t = new TurtleWalk();


var visualizer = new TurtleWalkVisualizer<Argb32>(new Vector2(2560, 1440), Color.White);

t.Walk(new() { Base = 10, WalkSequence = new SpigotPi() { Count = 10_000 } },
    (i, step) =>
    {
        Console.WriteLine($"Step: {i}");
        visualizer.DrawLine(step.PreviousPosition, step.CurrentPosition, Color.Black, 1f, 5f);
    });
var path = MakePath("spigot.png");
visualizer.Save(path);

Console.WriteLine($"saved at: {path}");

Console.WriteLine("Done");
Console.ReadKey();

static string MakePath(string fileName)
    => $@"{Environment.CurrentDirectory}\{fileName}";

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

public class SpigotPi : IWalkSequence
{

    uint[]? x;
    uint[]? r;
    uint carry = 0;
    int prevDigit = 0;
    public int Count { get; init; }

    public IEnumerator<(float value, Turtle.PenState penState)> GetEnumerator()
    {
        int l = (10 * Count / 3) + 1;
        var A = new List<int>(l);
        A.AddRange(Two(l));


        for (int i = 1; i <= Count; i++)
        {
            yield return (Spigot(i), Turtle.PenState.Down);
        }
            
        IEnumerable<int> Two(int count) 
        {
            for (int i = 0; i < count; i++)
            {
                yield return 2;
            }
        }
    
        uint Spigot(int digit) 
        {
            if (this.x == null) 
            {
                x = new uint[(Count+1) * 10 / 3 + 2];
                for (int j = 0; j < x.Length; j++)
                    x[j] = 20;
            }
            if (this.r == null) 
            {
                r = new uint[(Count + 1) * 10 / 3 + 2];
            }

            digit++;

            uint pDigit = 0;
            for (int i = prevDigit; i < digit; i++)
            {
                
                for (int j = 0; j < x.Length; j++)
                {
                    uint num = (uint)(x.Length - j - 1);
                    uint dem = num * 2 + 1;

                    x[j] += carry;

                    uint q = x[j] / dem;
                    r[j] = x[j] % dem;

                    carry = q * num;
                }


                pDigit = (x[x.Length - 1] / 10);


                r[x.Length - 1] = x[x.Length - 1] % 10; ;

                for (int j = 0; j < x.Length; j++)
                    x[j] = r[j] * 10;
            }
            prevDigit = digit;
            return pDigit;
        }
    
    }

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}
