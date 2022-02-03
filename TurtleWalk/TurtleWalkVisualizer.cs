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

    public void DrawLine(Vector2 start, Vector2 end, Color c, float thickness, float unitToPixels) 
    {
        var pathBuilder = new PathBuilder();
        pathBuilder.SetOrigin(new PointF(image.Width / 2f, image.Height / 2f));

        start *= unitToPixels;
        end *= unitToPixels;

        pathBuilder.AddLine(start.X, start.Y, end.X, end.Y);

        var path = pathBuilder.Build();
        image.Mutate(context => context.Draw(c, thickness, path));
    }

    public void Save(string filePath) 
    {
        image.Save(filePath);
    }

    static int RoundToInt(float val) 
        => (int)(val + 0.5f);
    
}
