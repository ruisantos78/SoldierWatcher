namespace SoldierWatcher.Service.Client.EventHandlers;

public class GeolocationEventArgs : EventArgs
{
    public string SerialNumber { get; }
    public double Latitude { get; }
    public double Longitude { get; }

    public GeolocationEventArgs(string serialNumber, double latitude, double longitude)
    {
        SerialNumber = serialNumber;
        Latitude = latitude;
        Longitude = longitude;
    }
}