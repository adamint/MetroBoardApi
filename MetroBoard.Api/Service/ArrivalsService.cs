using System.Diagnostics;
using MetroBoard.Api.Model;

namespace MetroBoard.Api.Service;

public class ArrivalsService
{
    private const string NomaStationCode = "B35";

    public Task<Matrix> DrawArrivalsScreenAsync() => DrawArrivalsScreenAsync(NomaStationCode);

    public async Task<Matrix> DrawArrivalsScreenAsync(string stationId)
    {
        var arrivals = (await GetArrivalsAsync(stationId))
            .Take(2)
            .ToArray();

        var matrix = Matrix.Create(Settings.SleepTimeSeconds);
        const int headerY = 4;
        matrix.TextLabels.Add(TextLabel.Create("LN", new Point(0, headerY)));
        matrix.TextLabels.Add(TextLabel.Create("DEST", new Point(15, headerY)));
        matrix.TextLabels.Add(TextLabel.Create("MIN", new Point(47, headerY)));

        var lines = arrivals.Select(arrival => arrival.Line)
            .Where(line => ColorUtils.LineAbbreviationsToColors.ContainsKey(line))
            .Select(line => ColorUtils.LineAbbreviationsToColors[line])
            .Take(2)
            .ToList();

        if (lines.Count == 1)
        {
            matrix.Pixels.AddRange(PolygonExtensions.CreateRect(0, headerY + 5, 64, 2, lines.Single().GetPaletteColor()));
        }
        else if (lines.Count == 2)
        {
            matrix.Pixels.AddRange(PolygonExtensions.CreateRect(0, headerY + 5, 64, 1, lines[0].GetPaletteColor()));
            matrix.Pixels.AddRange(PolygonExtensions.CreateRect(0, headerY + 6, 64, 1, lines[1].GetPaletteColor()));
        }

        if (arrivals.Length > 0)
        {
            const int lineHeight = 10;
            const int startingX = 1;
            const int startingY = 12;

            for (var i = 0; i < arrivals.Length; i++)
            {
                var arrival = arrivals[i];
                var lineColor = ColorUtils.LineAbbreviationsToColors.GetValueOrDefault(arrival.Line, Color.Green);

                if (string.IsNullOrEmpty(arrival.Min))
                {
                    arrival = arrival with
                    {
                        Min = "?"
                    };
                }

                var lineStartY = startingY + 4 + i * lineHeight;
                matrix.Pixels.AddRange(PolygonExtensions.CreateRect(startingX, startingY + i * lineHeight + 1, 2, 6, lineColor.GetPaletteColor()));
                matrix.TextLabels.Add(TextLabel.Create(GetDestinationName(arrival.Destination, arrival.Min), new Point(startingX + 4, lineStartY)));

                if (arrival.Min.Length == 3)
                {
                    matrix.TextLabels.Add(TextLabel.Create(arrival.Min, new Point(46, lineStartY)));
                }
                else if (arrival.Min.Length == 2)
                {
                    matrix.TextLabels.Add(TextLabel.Create(arrival.Min, new Point(46 + 6, lineStartY)));
                }
                else
                {
                    matrix.TextLabels.Add(TextLabel.Create(arrival.Min, new Point(46 + 12, lineStartY)));
                }
            }
        }
        else
        {
            matrix.TextLabels.Add(TextLabel.Create("None.", new Point(0, 14), color: 0xFF0000));
        }

        return matrix;

        static string GetDestinationName(string fullName, string minutes)
        {
            var length = fullName.Length + minutes.Length;
            if (length >= 10)
            {
                var maxNameLength = 9 - minutes.Length;
                return fullName[..maxNameLength];
            }

            return fullName;
        }
    }

    private async Task<TrainArrivalInformation[]> GetArrivalsAsync(string stationId)
    {
        var response = await HttpUtils.GetAsync<StationArrivalResponse>($"StationPrediction.svc/json/GetPrediction/{stationId}");
        Debug.Assert(response != null);
        return response.Trains;
    }
}