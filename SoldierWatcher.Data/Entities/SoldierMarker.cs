namespace SoldierWatcher.Data.Entities;

public record SoldierMarker
{
    public Guid Id { get; init; }
    public Soldier Soldier { get; init; } = Soldier.Empty;
    public string TrainingInfo { get; init; } = string.Empty;
    public Sensor Sensor { get; init; } = Sensor.Empty;
    public TrainingLocation TrainingLocation { get; init; } = TrainingLocation.Empty;

    public string SerialNumber { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}