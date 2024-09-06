namespace SoldierWatcher.Service.Client.Services.Structs;

internal struct Coordinates
{
    public static readonly Coordinates Empty = new Coordinates(0, 0);

    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public Coordinates(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }
}
