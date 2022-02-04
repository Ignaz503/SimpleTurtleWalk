using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using System.Numerics;
using SixLabors.ImageSharp.Formats.Gif;
using System.Runtime.CompilerServices;

namespace TurtleWalks;

public class TurtleWalkVisualizer<TPixel> where TPixel : unmanaged, IPixel<TPixel>
{

    Image<TPixel> image;

    public TurtleWalkVisualizer( Vector2 size,Color bgColor)
    {
        image = new Image<TPixel>(RoundToInt(size.X), RoundToInt(size.Y),bgColor.ToPixel<TPixel>());
    }

    public TurtleWalkVisualizer()
    {
        image = new Image<TPixel>(1, 1);
    }

    Image<TPixel> CreateImageForWalk(TurtleWalkResult results, Vector2 borderInPixel, Color bgColor, float pixelPerUnit)
    {
        var unscaledExtends = (results.Max - results.Min);
        var extendsWithoutBorder = unscaledExtends * pixelPerUnit;
        var extends = extendsWithoutBorder + 2f * borderInPixel;
        return new Image<TPixel>(CeilToInt(extends.X), CeilToInt(extends.Y), bgColor.ToPixel<TPixel>());
    }

    void SetupImage(TurtleWalkResult results, Vector2 borderInPixel, Color bgColor, float pixelPerUnit) 
    {
        image = CreateImageForWalk(results, borderInPixel, bgColor,pixelPerUnit);
    }

    public void Visualize(TurtleWalkResult results, Vector2 borderInPixel, Color bgColor, Color lineColor, float lineThickness,float pixelPerUnit) 
    {
        SetupImage(results, borderInPixel, bgColor,pixelPerUnit);
        var unscaledExtends = (results.Max - results.Min);
        var origin = borderInPixel;

        for (int i = 0; i < results.Count-1; i++)
        {
            var start = results[i];
            var end = results[i + 1];
            DrawLine(start,end,lineColor,lineThickness,pixelPerUnit,origin,results.Min,results.Max,unscaledExtends);
        }
    }

    public Image<TPixel> VisualizeAsGif(TurtleWalkResult results, Vector2 borderInPixel, Color bgColor, Color lineColor, float lineThickness, float pixelPerUnit, GifDrawInfo drawInfo) 
    {
        SetupImage(results, borderInPixel, bgColor, pixelPerUnit);
        var gif = new Image<TPixel>(image.Width, image.Height);

        var unscaledExtends = (results.Max - results.Min);
        var origin = borderInPixel;

        int count = 0;
        int marker = results.Count / drawInfo.NumberOfFrames;

        for (int i = 0; i < results.Count - 1; i++)
        {
            var start = results[i];
            var end = results[i + 1];
            DrawLine(start, end, lineColor, lineThickness, pixelPerUnit, origin, results.Min, results.Max, unscaledExtends);

            if (i % marker == 0) 
            {
                var copy = image.Frames.CloneFrame(0);
                copy.Frames.RootFrame.Metadata.GetFormatMetadata(GifFormat.Instance).FrameDelay = drawInfo.FrameDelayMilliseconds;
                gif.Frames.InsertFrame(count, copy.Frames.RootFrame);
                Console.WriteLine($"Adding Gif frame: {count++} from actual draw result {i}");
            }

        }
        image.Frames.RootFrame.Metadata.GetFormatMetadata(GifFormat.Instance).FrameDelay = drawInfo.LastFrameDealayMilliseconds;
        gif.Frames.InsertFrame(count, image.Frames.RootFrame);
        gif.Metadata.GetFormatMetadata(GifFormat.Instance).RepeatCount = 0;
        return gif;
    }

    public Image<TPixel> VisualizeTrailedWalk(TurtleWalkResult results, Vector2 borderInPixel, Color bgColor, Color lineColor, float lineThickness, float pixelPerUnit, GifDrawInfo drawInfo, int trailCount)
    {
        
        image = CreateImageForWalk(results, borderInPixel, bgColor, pixelPerUnit);
        var gif = new Image<TPixel>(image.Width, image.Height);

        var unscaledExtends = (results.Max - results.Min);
        var origin = borderInPixel;

        int count = 0;
        for (int i = 0; i < results.Count - 1; i++)
        {
            var drawOver = i - trailCount;
            if (drawOver >= 0) 
            {
                var startErase = results[drawOver];
                var enderase = results[drawOver + 1];
                bool drawLine = true;
                for (int j = drawOver + 1; j <= i; j++) 
                {
                    var compareStart = results[j];
                    var compareEnd = results[j + 1];

                    if(AlmostEquals((compareStart - startErase).LengthSquared(),0f) && AlmostEquals((compareEnd - enderase).LengthSquared(), 0f))
                        drawLine = false;
                    if (AlmostEquals((compareStart - enderase).LengthSquared(), 0f) && AlmostEquals((compareEnd - startErase).LengthSquared(), 0f))
                        drawLine = false;                 
                }

                if(drawLine)
                    DrawLine(startErase, enderase, bgColor, lineThickness, pixelPerUnit, origin, results.Min, results.Max, unscaledExtends);
            }

            var start = results[i];
            var end = results[i + 1];
            DrawLine(start, end, lineColor, lineThickness, pixelPerUnit, origin, results.Min, results.Max, unscaledExtends);

            var copy = image.Frames.CloneFrame(0);
            copy.Frames.RootFrame.Metadata.GetFormatMetadata(GifFormat.Instance).FrameDelay = drawInfo.FrameDelayMilliseconds;
            gif.Frames.InsertFrame(count, copy.Frames.RootFrame);
            Console.WriteLine($"Adding Gif frame: {count} from actual draw result {i}");
            count++;
            
        }
        image.Frames.RootFrame.Metadata.GetFormatMetadata(GifFormat.Instance).FrameDelay = drawInfo.LastFrameDealayMilliseconds;
        gif.Frames.InsertFrame(count, image.Frames.RootFrame);
        gif.Metadata.GetFormatMetadata(GifFormat.Instance).RepeatCount = 0;
        return gif;
    }

    void DrawLine(Vector2 start, Vector2 end, Color c, float thickness, float pixelPerUnit, Vector2 origin, Vector2 min, Vector2 max, Vector2 extends) 
    {

        start.X = Map(start.X, min.X, max.X, 0, extends.X);
        start.Y = Map(start.Y, min.Y, max.Y, 0, extends.Y);

        end.X = Map(end.X, min.X, max.X, 0, extends.X);
        end.Y = Map(end.Y, min.Y, max.Y, 0, extends.Y);

        DrawLine(start, end, c, thickness, pixelPerUnit, origin);
    }

    void DrawLine(Vector2 start, Vector2 end, Color c, float thickness, float pixelPerUnit, Vector2 origin) 
    {
        var pathBuilder = new PathBuilder();
        pathBuilder.SetOrigin(new PointF(origin.X, origin.Y));

        start *= pixelPerUnit;
        end *= pixelPerUnit;

        pathBuilder.AddLine(start.X, start.Y, end.X, end.Y);

        var path = pathBuilder.Build();
        image.Mutate(context => context.Draw(c, thickness, path));
    }

    public void DrawLine(Vector2 start, Vector2 end, Color c, float thickness, float pixelPerUnit) 
        => DrawLine(start, end, c, thickness, pixelPerUnit, new(image.Width / 2f, image.Height / 2f));

    public void Save(string filePath) 
    {
        image.Save(filePath);
    }

    static int RoundToInt(float val) 
        => (int)(val + 0.5f);

    static int CeilToInt(float val)
        => (int)(val + 1f);
    public static float Map(float x, float inMin, float inMax, float outMin, float outMax)
    {
        return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool AlmostEquals(float value1, float value2, float epsilon = 0.0000001f)
    {
        return Math.Abs(value1 - value2) < epsilon;
    }
}
