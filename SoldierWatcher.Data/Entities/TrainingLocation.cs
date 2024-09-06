namespace SoldierWatcher.Data.Entities;

public record TrainingLocation
{
    public static readonly TrainingLocation Empty = new();

    public Guid Id { get; init; } = Guid.Empty;
    public string LocationName { get; init; } = string.Empty;
    public double Latitude { get; init; }
    public double Longitude { get; init; }
}
