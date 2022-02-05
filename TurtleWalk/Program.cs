using TurtleWalks;
using System.Numerics;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;


const float PxPerUnit = 50f;
const int NumSequenceSteps = 1000;
const int Base = 10;
const string Directory = "Walk";
Vector2 Border = Vector2.One * 30;
//WalkInfo walkInfo = new() { Base = 6, WalkSequence = new EnumerableSqrt() { Count = 10_000, Value = 2 } };

Divide1To10ByAllNumbers(1000);


Console.WriteLine("Done");
Console.ReadKey();



void DrawAfterWalk(string name, WalkInfo walkInfo,Vector2 borderInPixel, DrawInfo drawInfo) 
{
    var t = new TurtleWalk();

    var res = t.Walk(walkInfo,
        (i) => { });

    if (res.Count <= 3) 
    {
        Console.WriteLine($"too short not drawing {res.Count}");
        return;
    }
    Console.WriteLine($"Drawing walk of lenght {res.Count}");
    var visualizer = new TurtleWalkVisualizer<Argb32>();
    visualizer.Visualize(res,borderInPixel, drawInfo.ImgaeBGColor,drawInfo.LineColor,drawInfo.LineThickness,drawInfo.PixelPerUnit);
    
    var path = MakePath(name);
    visualizer.Save(path);

    visualizer = null;
    res = null;
    GC.Collect();

    Console.WriteLine($"saved at: {path}");
}

void MakeGif(string name, WalkInfo walkInfo, Vector2 borderInPixel, DrawInfo drawInfo, GifDrawInfo gifInfo)
{
    var t = new TurtleWalk();

    var res = t.Walk(walkInfo,
        (i) => Console.WriteLine($"Step Draw After: {i}"));

    var visualizer = new TurtleWalkVisualizer<Argb32>();
    var gif = visualizer.VisualizeAsGif(res, borderInPixel, drawInfo.ImgaeBGColor, drawInfo.LineColor, drawInfo.LineThickness, drawInfo.PixelPerUnit,gifInfo);

    var path = MakePath(name);
    gif.SaveAsGif(path);

    Console.WriteLine($"saved at: {path}");
}

void MakeGifTrail(string name, WalkInfo walkInfo, Vector2 borderInPixel, DrawInfo drawInfo, GifDrawInfo gifInfo, int trail)
{
    var t = new TurtleWalk();

    var res = t.Walk(walkInfo,
        (i) => Console.WriteLine($"Step Draw After: {i}"));

    var visualizer = new TurtleWalkVisualizer<Argb32>();
    var gif = visualizer.VisualizeTrailedWalk(res, borderInPixel, drawInfo.ImgaeBGColor, drawInfo.LineColor, drawInfo.LineThickness, drawInfo.PixelPerUnit, gifInfo,trail);

    var path = MakePath(name);
    gif.SaveAsGif(path);

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

void DivisionEnumeration(int DivisorStart,int DivisorEnd, int Divisor) 
{
    WalkInfo info = new()
    {
        Base = Base,
    };
    DrawInfo drawInfo = new() { ImgaeBGColor = Color.Black, LineColor = Color.White, LineThickness = 1f, PixelPerUnit = PxPerUnit };

    for (int i = DivisorStart; i <= DivisorEnd; i++)
    {
        info.WalkSequence = new EnumerableDivision() { Count = NumSequenceSteps, Divident = i, Divisor = Divisor };
        DrawAfterWalk($"{i}over{Divisor}.png",info,Border,drawInfo);   
    }

}

void Divide1To10ByAllNumbers(int num) 
{
    for (int i = 1; i <= num; i++)
    {
        DivisionEnumeration(1, 10, i);
    }
}


static string MakePath(string fileName)
{
    if(!System.IO.Directory.Exists($@"{Environment.CurrentDirectory}\{Directory}"))
        System.IO.Directory.CreateDirectory($@"{Environment.CurrentDirectory}\{Directory}");

    return $@"{Environment.CurrentDirectory}\{Directory}\{fileName}";
}
