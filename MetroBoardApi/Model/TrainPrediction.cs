namespace MetroBoardApi.Model;

public record TrainPredictionResponse(TrainPrediction[] Trains);
public record TrainPrediction(
    string? Car,
    string Destination,
    string Group,
    string Line,
    string LocationCode,
    string LocationName,
    string Min,
    string? DestinationCode = null,
    string? DestinationName = null);