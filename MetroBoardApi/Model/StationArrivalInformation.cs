namespace MetroBoardApi.Model;

public record StationArrivalResponse(TrainArrivalInformation[] Trains);
public record TrainArrivalInformation(
    string? Car,
    string Destination,
    string? DestinationCode,
    string? DestinationName,
    string Group,
    string Line,
    string LocationCode,
    string LocationName,
    string Min);