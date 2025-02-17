namespace MetroBoard.Api.Model;

public enum Color
{
    Blue = 1,
    Green = 2,
    Orange = 3,
    Red = 4,
    Silver = 5,
    Yellow = 6,
    White = 7
}

public static class ColorUtils
{
    public static Dictionary<int, int> Palette = new()
    {
        { 0, 0x000000 },
        { (int)Color.Blue, 0x0000FF },
        { (int)Color.Green, 0x00FF00 },
        { (int)Color.Orange, 0xC76E00 },
        { (int)Color.Red, 0xFF0000 },
        { (int)Color.Silver, 0xC0C0C0 },
        { (int)Color.Yellow, 0xFFFF00 },
        { (int)Color.White, 0xf2f5f5 }
    };

    public static int GetPaletteColor(this Color color) => Palette.Keys.ToList().IndexOf((int)color);

    public static readonly Dictionary<string, Color> LineAbbreviationsToColors = new()
    {
        { "", Color.White },
        { "BL", Color.Blue },
        { "GR", Color.Green },
        { "OR", Color.Orange },
        { "RD", Color.Red },
        { "SV", Color.Silver },
        { "YL", Color.Yellow },
        { "WT", Color.White }
    };
}