using TurtleWalks;
using System.Numerics;
using SixLabors.ImageSharp.PixelFormats;
using System.Collections;
using SixLabors.ImageSharp;

WalkInfo walkInfo = new() { Base = 4, WalkSequence = new EnumerableSqrt() { Count = 10_000, Value = 2 } };

//DrawDuringWalk("spigot.png",walkInfo, new Vector2(2560, 1440),new DrawInfo() { PixelPerUnit = 5f});
//DrawAfterWalk("spigotAfter.png",walkInfo, Vector2.One * 30,new DrawInfo() { PixelPerUnit = 5f });
MakeGif("sqrt.gif",walkInfo, Vector2.One * 30,new DrawInfo() { PixelPerUnit = 5f }, new GifDrawInfo() { FrameDelayMilliseconds = 16, NumberOfFrames = 300, LastFrameDealayMilliseconds = 100});
//MakeGifTrail("sqrtTrail.gif",walkInfo, Vector2.One * 30,new DrawInfo() { PixelPerUnit = 5f }, new GifDrawInfo() { FrameDelayMilliseconds = 1, NumberOfFrames = 0/*is ignored*/, LastFrameDealayMilliseconds = 100},7);

Console.WriteLine("Done");
Console.ReadKey();

void DrawAfterWalk(string name, WalkInfo walkInfo,Vector2 borderInPixel, DrawInfo drawInfo) 
{
    var t = new TurtleWalk();

    var res = t.Walk(walkInfo,
        (i) => Console.WriteLine($"Step Draw After: {i}"));

    var visualizer = new TurtleWalkVisualizer<Argb32>();
    visualizer.Visualize(res,borderInPixel, drawInfo.ImgaeBGColor,drawInfo.LineColor,drawInfo.LineThickness,drawInfo.PixelPerUnit);
    
    var path = MakePath(name);
    visualizer.Save(path);

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


static string MakePath(string fileName)
    => $@"{Environment.CurrentDirectory}\{fileName}";


public class GoldenRatio : IWalkSequence
{
    public int Count { get; init; }

    public IEnumerator<(float value, Turtle.PenState penState)> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}