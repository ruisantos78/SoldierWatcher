using CommunityToolkit.Mvvm.ComponentModel;
using SoldierWatcher.Data.Entities;
using SoldierWatcher.Data.Repositories;
using SoldierWatcher.Models;
using SoldierWatcher.Service.Client.EventHandlers;
using SoldierWatcher.Service.Client.Interfaces;
using System.Collections.ObjectModel;

namespace SoldierWatcher.ViewModels;

public partial class TrainingFieldViewModel : ObservableObject
{
    [ObservableProperty] IReadOnlyList<TrainingLocation> _trainingLocations = [];
    [ObservableProperty] TrainingLocation? _selectedTrainingLocation;
    [ObservableProperty] ObservableCollection<SoldierMarkerModel> _soldiers = [];
    [ObservableProperty] SoldierMarkerModel? _selectedSoldier;

    private readonly CancellationTokenSource cancellationTokenSource;
    private readonly ITrainingLocationsRepository trainingLocationsRepository;
    private readonly ISoldierMarkerRepository soldierMarkerRepository;
    private readonly IGeolocationService geolocationService;

    public TrainingFieldViewModel(
        CancellationTokenSource cancellationTokenSource,
        ITrainingLocationsRepository trainingLocationsRepository,
        ISoldierMarkerRepository soldierMarkerRepository,
        IGeolocationService geolocationService)
    {
        this.cancellationTokenSource = cancellationTokenSource;
        this.trainingLocationsRepository = trainingLocationsRepository;
        this.soldierMarkerRepository = soldierMarkerRepository;

        this.geolocationService = geolocationService;
        this.geolocationService.GeolocationUpdated += OnGeolocationServiceGeolocationUpdated;

        InitializeContext();
    }

    private async void InitializeContext()
    {
        await Task.WhenAll(
            trainingLocationsRepository.GetTrainingLocationsAsync(cancellationTokenSource.Token).ContinueWith(SetTrainingLocations),
            Task.CompletedTask // Placeholder for other async tasks
        );

        await geolocationService.ConnectAsync("fake://host", cancellationTokenSource.Token);
    }


    async partial void OnSelectedTrainingLocationChanged(TrainingLocation? value)
    {
        // Remove listners from soldier fro another location
        Soldiers?.ToList().ForEach(x => geolocationService.RemoveListener(x.SerialNumber));

        if (value is null)
        {
            Soldiers = new();
            return;
        }

        var soldiers = await soldierMarkerRepository.GetSoldiersByTrainingLocation(value, cancellationTokenSource.Token);
        Soldiers = new(soldiers.Select(x => new SoldierMarkerModel(x)));
        Soldiers?.ToList().ForEach(x => geolocationService.AddListener(x.SerialNumber));
    }

    private void OnGeolocationServiceGeolocationUpdated(object? sender, GeolocationEventArgs e)
    {
        var soldier = Soldiers?.FirstOrDefault(x => x.SerialNumber == e.SerialNumber);
        if (soldier is null)
            return;

        soldier.UpdateGeolocation(e.Latitude, e.Longitude);
        soldierMarkerRepository.UpdateSoldierLocation(soldier, cancellationTokenSource.Token);
    }

    private void SetTrainingLocations(Task<IReadOnlyList<TrainingLocation>> task)
    {
        TrainingLocations = task.Result;
        SelectedTrainingLocation = TrainingLocations.FirstOrDefault();
    }
}
