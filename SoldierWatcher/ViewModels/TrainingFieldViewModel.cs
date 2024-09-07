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
        ClearListeners();

        if (value is null)
        {
            Soldiers = new();
            return;
        }

        var soldiers = await soldierMarkerRepository.GetSoldiersByTrainingLocationAsync(value, cancellationTokenSource.Token);
        Soldiers = new(soldiers.Select(x => new SoldierMarkerModel(x)));

        UpdateListeners();
    }

    private void OnGeolocationServiceGeolocationUpdated(object? sender, GeolocationEventArgs e)
    {
        var soldier = Soldiers?.FirstOrDefault(x => x.SerialNumber == e.SerialNumber);
        if (soldier is null)
            return;

        soldier.UpdateGeolocation(e.Latitude, e.Longitude);
        soldierMarkerRepository.UpdateSoldierLocationAsync(soldier, cancellationTokenSource.Token);
    }


    private void ClearListeners()
    {
        var tasks = Soldiers?.Select(x => geolocationService.RemoveListenerAsync(x.SerialNumber))
            .ToArray();

        if (tasks is not null && tasks.Length > 0)
            Task.WaitAll(tasks);
    }

    private void UpdateListeners()
    {
        var tasks = Soldiers?.Select(x => geolocationService.AddListenerAsync(x.SerialNumber))
            .ToArray();

        if (tasks is not null && tasks.Length > 0)
            Task.WaitAll(tasks);
    }

    private void SetTrainingLocations(Task<IReadOnlyList<TrainingLocation>> task)
    {
        TrainingLocations = task.Result;
        SelectedTrainingLocation = TrainingLocations.FirstOrDefault();
    }
}
