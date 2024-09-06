using CommunityToolkit.Mvvm.ComponentModel;
using SoldierWatcher.Data.Entities;

namespace SoldierWatcher.Models;

public partial class SoldierMarkerModel: ObservableObject
{
    private DateTime _lastUpdate = DateTime.Now;
    private readonly SoldierMarker model;
    
    public string SoldierName => model.Soldier.Name;
    public string CountryName => model.Soldier.Country.Name;
    public string Rank => model.Soldier.Rank.ToString();
    public string SerialNumber => model.SerialNumber;

    public double Latitude
    {
        get => model.Latitude;
        private set => SetProperty(model.Latitude, value, _ => model.Latitude = value);
    }

    public double Longitude
    {
        get => model.Longitude;
        private set => SetProperty(model.Longitude, value, _ => model.Longitude = value);
    }

    public DateTime LastUpdate
    {
        get => _lastUpdate;
        private set => SetProperty(ref _lastUpdate, value);
    }

    [ObservableProperty] bool _isChecked = true;

    public event EventHandler? IsCheckedChanged;
    public event EventHandler? GeolocationChanged;

    public SoldierMarkerModel(SoldierMarker model)
    {
        this.model = model;
    }

    partial void OnIsCheckedChanged(bool value)
    {
        IsCheckedChanged?.Invoke(this, new EventArgs());
    }

    public void UpdateGeolocation(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
        LastUpdate = DateTime.Now;

        GeolocationChanged?.Invoke(this, new EventArgs());         
    }

    public static implicit operator SoldierMarker(SoldierMarkerModel model) => model.model;
}
