using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using System.Numerics;

namespace TurtleWalks;

public class TurtleWalkVisualizer<TPixel> where TPixel : unmanaged, IPixel<TPixel>
{

    Image<TPixel> image;

    public TurtleWalkVisualizer( Vector2 size,Color bgColor)
    {
        image = new Image<TPixel>(RoundToInt(size.X), RoundToInt(size.Y),bgColor.ToPixel<TPixel>());
    }

    public TurtleWalkVisualizer(TurtleWalkResult results, Vector2 borderInPixel, Color bgColor, Color lineColor, float lineThickness,float pixelPerUnit) 
    {
        var unscaledExtends = (results.Max - results.Min);
        var extendsWithoutBorder =  unscaledExtends * pixelPerUnit;
        var extends= extendsWithoutBorder + 2f * borderInPixel;

        image = new Image<TPixel>(CeilToInt(extends.X), CeilToInt(extends.Y), bgColor.ToPixel<TPixel>());

        var origin = borderInPixel;

        for (int i = 0; i < results.Count-1; i++)
        {
            var start = results[i];
            var end = results[i + 1];

            start.X = Map(start.X, results.Min.X, results.Max.X, 0, unscaledExtends.X);
            start.Y = Map(start.Y, results.Min.Y, results.Max.Y, 0, unscaledExtends.Y);

            end.X = Map(end.X, results.Min.X, results.Max.X, 0, unscaledExtends.X);
            end.Y = Map(end.Y, results.Min.Y, results.Max.Y, 0, unscaledExtends.Y);

            DrawLine(start, end, lineColor, lineThickness, pixelPerUnit, origin);
        }
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
}
