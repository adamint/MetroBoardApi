namespace MetroBoardApi.Model;

[Serializable]
public record Matrix(List<Pixel> Pixels, List<Polygon> Polygons, List<TextLabel> TextLabels)
{
    public static Matrix Create()
    {
        return new Matrix([], [], []);
    }
}

public record Pixel(Point Position, int Color);
public record Polygon(Point[] Points, int Color);
public record TextLabel(string Text, Point Position, int? Color = null, int? BackgroundColor = null);
public record Point(int X, int Y);