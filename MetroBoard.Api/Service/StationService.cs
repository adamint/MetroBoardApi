using System.Diagnostics;
using MetroBoard.Api.Model;

namespace MetroBoard.Api.Service;

public class StationService
{
    private StationsInfo? _stationsInfo;

    public async Task<Matrix> DrawStationScreenAsync(int remainingTimesToDraw)
    {
        const int displayWidth = 64;
        const int displayHeight = 32;
        _stationsInfo ??= await GetStationsInfo(displayWidth, displayHeight);

        var matrix = Matrix.Create(ScreenService.SleepTimeSeconds);

        // draw stations
        foreach (var (station, coordinate) in _stationsInfo.StationsToNormalizedCoordinates)
        {
            var fill = GetSelectedLineFill(station, remainingTimesToDraw);
            Console.WriteLine($"Station: {station.Name} - {station.Code}, ({coordinate.X}, {coordinate.Y})");
            matrix.Pixels.Add(new Pixel(new Point(coordinate.X, coordinate.Y), fill));
        }

        // draw train count
        var trainPositions = await GetTrainPositionsAsync();
        DrawTrainCount(matrix, trainPositions);

        // blink stations with an arrival in the next minute
        var trainPredictions = await GetTrainPredictionsAsync();
        var stationsWithArrivalsNow = trainPredictions
            .Where(t => t.Min == "BRD")
            .ToList();

        foreach (var prediction in stationsWithArrivalsNow)
        {
            var station = _stationsInfo.StationsToNormalizedCoordinates.Keys.FirstOrDefault(s => s.Code == prediction.LocationCode);
            if (station is not null)
            {
                var coordinate = _stationsInfo.StationsToNormalizedCoordinates[station];
                matrix.PointsToBlink.Add(new Point(coordinate.X, coordinate.Y));
            }
        }

        return matrix;

        static int GetSelectedLineFill(Station station, int remainingTimesToDraw)
        {
            var lines = new List<string?> { station.LineCode1, station.LineCode2, station.LineCode3, station.LineCode4 }
                .Where(s => !string.IsNullOrEmpty(s))
                .Cast<string>()
                .ToList();

            var selectedLine = lines[remainingTimesToDraw % lines.Count];
            return ColorUtils.LineAbbreviationsToColors[selectedLine].GetPaletteColor();
        }
    }

    private static async Task<StationsInfo> GetStationsInfo(int displayWidth, int displayHeight)
    {
        var stations = await GetStationsAsync();
        var stationsToNormalizedCoordinates = GetStationNormalizedCoordinates(stations, displayWidth, displayHeight);
        return new StationsInfo(stationsToNormalizedCoordinates);
    }

    private static Dictionary<Station, NormalizedCoordinate> GetStationNormalizedCoordinates(Station[] stations, int displayWidth, int displayHeight)
    {
        var stationsByCode = stations.ToDictionary(s => s.Code);

        var minimumLon = stations.Min(s => s.Lon);
        var maximumLon = stations.Max(s => s.Lon);
        var displayXMin = 0 + 1;
        var displayXMax = displayWidth - 2;

        var minimumLat = stations.Min(s => s.Lat);
        var maximumLat = stations.Max(s => s.Lat);
        var displayYMin = 0 + 1;
        var displayYMax = displayHeight - 2;

        double ScaleXCoordinate(double x)
        {
            return (x - minimumLon) / (maximumLon - minimumLon) * (displayXMax - displayXMin) + displayXMin;
        }

        double ScaleYCoordinate(double y)
        {
            return displayYMax - (y - minimumLat) / (maximumLat - minimumLat) * (displayYMax - displayYMin) + displayYMin - 1;
        }

        var stationNormalizedCoordinates = new Dictionary<Station, NormalizedCoordinate>();
        var uniqueCoordinatesToNormalizedCoordinates = new Dictionary<Coordinate, NormalizedCoordinate>();

        foreach (var station in stations)
        {
            var x = ScaleXCoordinate(station.Lon);
            var y = ScaleYCoordinate(station.Lat);

            var disambiguatedCoordinates = DisambiguateCoordinates(x, y, uniqueCoordinatesToNormalizedCoordinates);
            var uniqueCoordinate = new NormalizedCoordinate(Convert.ToInt32(disambiguatedCoordinates.X), Convert.ToInt32(disambiguatedCoordinates.Y));
            uniqueCoordinatesToNormalizedCoordinates[new Coordinate(Convert.ToInt32(y), Convert.ToInt32(x))] = disambiguatedCoordinates;
            stationNormalizedCoordinates[station] = uniqueCoordinate;

            static NormalizedCoordinate DisambiguateCoordinates(double x, double y, Dictionary<Coordinate, NormalizedCoordinate> uniqueCoordinatesToNormalizedCoordinates)
            {
                if (uniqueCoordinatesToNormalizedCoordinates.TryGetValue(
                        new Coordinate(Convert.ToInt32(x), Convert.ToInt32(y)), out var existingCoordinate))
                {
                    var newX = x;
                    var newY = y;

                    bool anyChange = false;
                    if (x > existingCoordinate.X)
                    {
                        newX += 1;
                        anyChange = true;
                    }
                    else if (x < existingCoordinate.X)
                    {
                        newX -= 1;
                        anyChange = true;
                    }

                    if (y > existingCoordinate.Y)
                    {
                        newY += 1;
                        anyChange = true;
                    }
                    else if (y < existingCoordinate.Y)
                    {
                        newY -= 1;
                        anyChange = true;
                    }

                    if (anyChange)
                    {
                        return DisambiguateCoordinates(newX, newY, uniqueCoordinatesToNormalizedCoordinates);
                    }
                    else
                    {
                        return new NormalizedCoordinate(Convert.ToInt32(newX), Convert.ToInt32(newY));
                    }
                }
                else
                {
                    return new NormalizedCoordinate(Convert.ToInt32(x), Convert.ToInt32(y));
                }
            }
        }

        Console.WriteLine("Confirming there are no duplicate coordinates...");

        var seen = new HashSet<NormalizedCoordinate>();
        var notUnique = new List<NormalizedCoordinate>();
        foreach (var (station, coord) in stationNormalizedCoordinates)
        {
            if (!seen.Add(coord))
            {
                if (stationsByCode[station.Code].StationTogether1 == "")
                {
                    notUnique.Add(coord);
                }
            }
        }

        if (notUnique.Count > 0)
        {
            Console.WriteLine("There are duplicate coordinates for the following stations:");
            foreach (var coord in notUnique)
            {
                foreach (var (station, stationCoord) in stationNormalizedCoordinates)
                {
                    if (stationCoord == coord)
                    {
                        Console.WriteLine($"{station.Name} at {coord}");
                    }
                }
            }
        }
        else Console.WriteLine("There are no duplicate coordinates.");

        return stationNormalizedCoordinates;
    }

    private static void DrawTrainCount(Matrix matrix, TrainPosition[] trainPositions)
    {
        var xOffset = trainPositions.Length < 100 ? 2 : 0;
        var trainXStart = 15 + xOffset;
        const int trainYStart = 2;
        const int trainYEnd = 9;
        const int windowWidth = 2;

        int windowXStart;
        int windowYStart;

        if (trainPositions.Length >= 100)
        {
            trainXStart += 6;

            matrix.Polygons.Add(new Polygon([
                new Point(trainXStart, trainYStart),
                new Point(trainXStart + 6, trainYStart),
                new Point(trainXStart + 13, trainYEnd),
                new Point(trainXStart + 6, trainYEnd),
                new Point(trainXStart, trainYEnd)
            ], 1));

            windowXStart = trainXStart + 2;
            windowYStart = trainYStart + 2;
        }
        else
        {
            matrix.Polygons.Add(new Polygon([
                new Point(trainXStart, trainYStart),
                new Point(trainXStart + 10, trainYStart),
                new Point(trainXStart + 17, trainYEnd),
                new Point(trainXStart + 10, trainYEnd),
                new Point(trainXStart, trainYEnd)
            ], Color.Blue.GetPaletteColor()));

            windowXStart = trainXStart + 7;
            windowYStart = trainYStart + 2;
        }


        matrix.Polygons.Add(new Polygon([
            new Point(windowXStart, windowYStart),
            new Point(windowXStart + windowWidth, windowYStart),
            new Point(windowXStart + windowWidth, windowYStart + windowWidth),
            new Point(windowXStart, windowYStart + windowWidth)
        ], 1));

        int textXStart = 3 + xOffset;
        int textYStart = 6;
        matrix.TextLabels.Add(TextLabel.Create(trainPositions.Length.ToString(), new Point(textXStart, textYStart)));
    }

    private static async Task<TrainPosition[]> GetTrainPositionsAsync()
    {
        var response = await HttpUtils.GetAsync<TrainPositionResponse>("/TrainPositions/TrainPositions?contentType=json");
        Debug.Assert(response is not null);

        return response.TrainPositions;
    }

    private static async Task<Station[]> GetStationsAsync()
    {
        var response = await HttpUtils.GetAsync<StationResponse>("/Rail.svc/json/jStations");
        Debug.Assert(response is not null);

        return response.Stations;
    }

    private static async Task<TrainPrediction[]> GetTrainPredictionsAsync()
    {
        var response = await HttpUtils.GetAsync<TrainPredictionResponse>("/StationPrediction.svc/json/GetPrediction/All");
        Debug.Assert(response is not null);

        return response.Trains;
    }
}

public record Coordinate(double Lat, double Lon);
public record NormalizedCoordinate(int X, int Y);

public record StationsInfo(Dictionary<Station, NormalizedCoordinate> StationsToNormalizedCoordinates);