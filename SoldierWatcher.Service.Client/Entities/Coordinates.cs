namespace SoldierWatcher.Service.Client.Entities;

public struct Coordinates
{
    public static readonly Coordinates Empty = new Coordinates(0, 0);

    public double Latitude { get; internal set; }
    public double Longitude { get; internal set; }

    public Coordinates(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }
}
