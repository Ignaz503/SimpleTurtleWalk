using TurtleWalks;
using System.Numerics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Collections;
using System.Runtime.CompilerServices;

WalkInfo walkInfo = new() { Base = 10, WalkSequence = new SpigotPi() { Count = 10_000 } };

//DrawDuringWalk("spigot.png",walkInfo, new Vector2(2560, 1440),new DrawInfo() { PixelPerUnit = 5f});
DrawAfterWalk("spigotAfter.png",walkInfo, Vector2.One * 30,new DrawInfo() { PixelPerUnit = 5f });

Console.WriteLine("Done");
Console.ReadKey();

void DrawAfterWalk(string name, WalkInfo walkInfo,Vector2 borderInPixel, DrawInfo drawInfo) 
{
    var t = new TurtleWalk();

    var res = t.Walk(walkInfo,
        (i) => Console.WriteLine($"Step Draw After: {i}"));

    var visualizer = new TurtleWalkVisualizer<Argb32>(res,borderInPixel, drawInfo.ImgaeBGColor,drawInfo.LineColor,drawInfo.LineThickness,drawInfo.PixelPerUnit);
    
    var path = MakePath(name);
    visualizer.Save(path);

    Console.WriteLine($"saved at: {path}");
}

void DrawDuringWalk(string name,WalkInfo walkInfo,Vector2 imageSize, DrawInfo drawInfo) 
{
    var t = new TurtleWalk();

    var visualizer = new TurtleWalkVisualizer<Argb32>(imageSize, drawInfo.ImgaeBGColor);

    t.Walk(walkInfo,
        (i, step) =>
        {
            Console.WriteLine($"Step Draw During Walk: {i}");
            visualizer.DrawLine(step.PreviousPosition, step.CurrentPosition, drawInfo.LineColor,drawInfo.LineThickness,  drawInfo.PixelPerUnit);
        });
    var path = MakePath(name);
    visualizer.Save(path);

    Console.WriteLine($"saved at: {path}");
}


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

        for (int i = 1; i <= Count; i++)
        {
            var res = Spigot(i);

            if (i == Count) //make it reuseable
                Reset();

            yield return (res, Turtle.PenState.Down);
        }
    }

    void Reset() 
    {
        x = null;
        r = null;
        carry = 0;
        prevDigit = 0;
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

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}

public struct DrawInfo 
{
    public Color ImgaeBGColor { get; init; } = Color.White;
    public Color LineColor { get; init; } = Color.Black;
    public float LineThickness { get; init; } = 1f;
    public float PixelPerUnit { get; init; } = 50f;
}