using MetroBoard.Api.Model;

namespace MetroBoard.Api;

public static class PolygonExtensions
{
    public static IEnumerable<Pixel> CreateRect(int x, int y, int width, int height, int color)
    {
        for (var i = x; i < x + width; i++)
        {
            for (var j = y; j < y + height; j++)
            {
                yield return new Pixel(new Point(i, j), color);
            }
        }
    }
}