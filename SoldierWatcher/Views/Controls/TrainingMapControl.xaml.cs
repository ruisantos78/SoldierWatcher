using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using SoldierWatcher.Data.Entities;
using SoldierWatcher.Models;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Duration = System.Windows.Duration;

namespace SoldierWatcher.Views.Controls;

public partial class TrainingMapControl : UserControl
{
    public static readonly DependencyProperty LatitudeProperty = DependencyProperty.Register("Latitude", typeof(double), typeof(TrainingMapControl),
        new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender, OnLatitudePropertyChanged));

    public static readonly DependencyProperty LongitudeProperty = DependencyProperty.Register("Longitude", typeof(double), typeof(TrainingMapControl),
        new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender, OnLongitudePropertyChanged));

    public static readonly DependencyProperty ItemSourceProperty = DependencyProperty.Register("ItemSource", typeof(ObservableCollection<SoldierMarkerModel>), typeof(TrainingMapControl),
        new FrameworkPropertyMetadata(new ObservableCollection<SoldierMarkerModel>(), FrameworkPropertyMetadataOptions.AffectsRender, OnItemSourcePropertyChanged));

    public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(SoldierMarkerModel), typeof(TrainingMapControl),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemPropertyChanged));

    public static readonly DependencyProperty MarkerStyleProperty = DependencyProperty.Register("MarkerStyle", typeof(Style), typeof(TrainingMapControl),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

    private static void OnLatitudePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is TrainingMapControl mapControl && e.NewValue is double latitude)
            mapControl.OnLatitudeChanged(latitude);
    }

    private static void OnLongitudePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is TrainingMapControl mapControl && e.NewValue is double longitude)
            mapControl.OnLongitudeChanged(longitude);
    }

    private static void OnItemSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is TrainingMapControl mapControl)
        {
            var oldModel = e.OldValue as ObservableCollection<SoldierMarkerModel>;
            if (e.NewValue is ObservableCollection<SoldierMarkerModel> newModels)
            {
                mapControl.OnItemSourceChanged(oldModel, newModels);
            }
        }
    }

    private static void OnSelectedItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is TrainingMapControl mapControl)
            mapControl.OnSelectedItemChanged(e.NewValue as SoldierMarkerModel);
    }


    public double Latitude
    {
        get { return (double)GetValue(LatitudeProperty); }
        set { SetValue(LatitudeProperty, value); }
    }

    public double Longitude
    {
        get { return (double)GetValue(LongitudeProperty); }
        set { SetValue(LongitudeProperty, value); }
    }

    public ObservableCollection<SoldierMarkerModel> ItemSource
    {
        get { return (ObservableCollection<SoldierMarkerModel>)GetValue(ItemSourceProperty); }
        set { SetValue(ItemSourceProperty, value); }
    }

    public SoldierMarkerModel? SelectedItem
    {
        get { return (SoldierMarkerModel?)GetValue(SelectedItemProperty); }
        set { SetValue(SelectedItemProperty, value); }
    }

    public Style MarkerStyle
    {
        get { return (Style)GetValue(MarkerStyleProperty); }
        set { SetValue(MarkerStyleProperty, value); }
    }


    public TrainingMapControl()
    {
        InitializeComponent();
        InitalizeMap();
    }


    private void InitalizeMap()
    {
        gmapControl.MapProvider = GMapProviders.BingOSMap;

        gmapControl.MinZoom = 2;
        gmapControl.MaxZoom = 22;
        gmapControl.Zoom = 18;

        gmapControl.ShowCenter = false;
        gmapControl.CanDragMap = true;
    }


    private void OnLatitudeChanged(double latitude)
    {
        gmapControl.Position = new PointLatLng(latitude, Longitude);
    }

    private void OnLongitudeChanged(double longitude)
    {
        gmapControl.Position = new PointLatLng(Latitude, longitude);
    }

    private void OnItemSourceChanged(ObservableCollection<SoldierMarkerModel>? oldModels, ObservableCollection<SoldierMarkerModel> newModels)
    {
        if (oldModels is not null)
        {
            oldModels.CollectionChanged -= OnItemsSourceCollectionChanged;
            RemoveMarkers(oldModels);
        }

        AddMarkers(newModels);
        newModels.CollectionChanged += OnItemsSourceCollectionChanged;
    }

    private void OnItemsSourceCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                AddMarkers(e.NewItems);
                break;

            case NotifyCollectionChangedAction.Remove:
                RemoveMarkers(e.OldItems);
                break;
        }
    }

    private void OnMarkerSelected(object sender, RoutedEventArgs e)
    {
        SelectedItem = (sender as RadioButton)?.DataContext as SoldierMarkerModel;
    }

    private void OnMarkerIsCheckedChanged(object? sender, EventArgs e)
    {
        if (sender is not SoldierMarkerModel model)
            return;

        var marker = gmapControl.Markers.FirstOrDefault(m => m.Tag == model);
        if (marker is not null)
            marker.Shape.Visibility = model.IsChecked ? Visibility.Visible : Visibility.Hidden;
    }

    private void OnMarkerGeolocationChanged(object? sender, EventArgs e)
    {
        if (sender is not SoldierMarkerModel model)
            return;

        MoveMarker(model);
    }

    private void OnSelectedItemChanged(SoldierMarkerModel? model)
    {
        var selected = gmapControl.Markers.Select(m => m.Shape)
            .OfType<RadioButton>()
            .FirstOrDefault(x => x.IsChecked is true);

        if (selected?.DataContext == model)
            return;

        if (model is null && selected is not null)
        {
            selected.IsChecked = false;
            return;
        }

        var marker = gmapControl.Markers.FirstOrDefault(m => m.Tag == model);
        gmapControl.Markers.ToList().ForEach(m => m.ZIndex = m == marker ? 100 : 0);
        if (marker?.Shape is RadioButton button)
        {
            button.IsChecked = true;
        }
    }


    private void AddMarkers(System.Collections.IList? models)
    {
        gmapControl.Markers.Clear();

        if (models is null)
            return;

        foreach (var model in models.OfType<SoldierMarkerModel>())
        {
            var shape = new RadioButton
            {
                GroupName = "SoldierMarkers",
                Style = MarkerStyle,
                DataContext = model,
            };

            gmapControl.Markers.Add(new GMapMarker(new PointLatLng(model.Latitude, model.Longitude))
            {
                Shape = shape,
                Tag = model
            });

            shape.Checked += OnMarkerSelected;
            model.IsCheckedChanged += OnMarkerIsCheckedChanged;
            model.GeolocationChanged += OnMarkerGeolocationChanged;
        }
    }

    private void RemoveMarkers(System.Collections.IList? models)
    {
        if (models is null)
            return;

        foreach (var model in models.OfType<SoldierMarkerModel>())
        {
            model.IsCheckedChanged -= OnMarkerIsCheckedChanged;

            var marker = gmapControl.Markers.FirstOrDefault(m => m.Tag == model);
            if (marker is not null)
                gmapControl.Markers.Remove(marker);
        }
    }

    private void MoveMarker(SoldierMarkerModel model)
    {
        var marker = gmapControl.Markers.FirstOrDefault(m => m.Tag == model);
        if (marker == null)
            return;

        var start = marker.Position;
        var end = new PointLatLng(model.Latitude, model.Longitude);

        if (start.Lng == 0 && start.Lat == 0)
        {
            marker.Position = end;
            return;
        }

        var duration = 100.0;
        var stepInterval = 5;
        var stepCount = duration / stepInterval;
        var currentStep = 0;

        var timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(stepInterval)
        };

        timer.Tick += (s, e) =>
        {
            if (currentStep < stepCount)
            {
                currentStep++;
                double t = (double)currentStep / stepCount;
                var currentLat = start.Lat + t * (end.Lat - start.Lat);
                var currentLng = start.Lng + t * (end.Lng - start.Lng);
                marker.Position = new PointLatLng(currentLat, currentLng);
            }
            else
            {
                timer.Stop();
                marker.Position = end;
            }
        };

        timer.Start();
    }

}
