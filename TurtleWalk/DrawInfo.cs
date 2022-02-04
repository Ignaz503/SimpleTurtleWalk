using SixLabors.ImageSharp;

public struct DrawInfo 
{
    public Color ImgaeBGColor { get; init; } = Color.White;
    public Color LineColor { get; init; } = Color.Black;
    public float LineThickness { get; init; } = 1f;
    public float PixelPerUnit { get; init; } = 50f;
}