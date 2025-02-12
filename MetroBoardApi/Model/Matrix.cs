namespace MetroBoardApi.Model;
using PixelColor = Color;

[Serializable]
public record Matrix(
    List<Pixel> Pixels,
    List<Polygon> Polygons,
    List<TextLabel> TextLabels,
    int SleepTime,
    List<Point> PointsToBlink,
    double BlinkRate,
    string ApplicationVersion)
{
    private static readonly string Version = Guid.NewGuid().ToString();

    public static Matrix Create(int sleepTime)
    {
        return new Matrix([], [], [], sleepTime, [], 0.75, Version);
    }
}

public record Pixel(Point Position, int Color);
public record Polygon(Point[] Points, int Color);

public record TextLabel(string Text, Point Position, int Color, int? BackgroundColor = null)
{
    public static TextLabel Create(string text, Point position, int? color = null, int? backgroundColor = null)
    {
        return new TextLabel(text, position, color ?? ColorUtils.Palette[(int)PixelColor.White], backgroundColor);
    }
}

public record Point(int X, int Y);