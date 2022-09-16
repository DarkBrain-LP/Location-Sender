using LocationSender.Services;

namespace LocationSender;

public partial class MainPage : ContentPage
{
	int count = 0;
    int time = 0;
    Location location;
    LocationService service;

    public MainPage()
	{
		InitializeComponent();
	}

    private async void OnSendBtnClicked(object sender, EventArgs e)
    {
        var periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(time));
        while (await periodicTimer.WaitForNextTickAsync())
        {
            // Place function in here..
            service.SendLocation();
        }
        //count++;

        //if (count == 1)
        //	CounterBtn.Text = $"Clicked {count} time";
        //else
        //	CounterBtn.Text = $"Clicked {count} times";

        //SemanticScreenReader.Announce(CounterBtn.Text);
    }

    //public void OnSetTimeIntervalClicked(object sender, EventArgs e)
    //{
    //    time = (int)TimeSlider.Value;

    //    try
    //    {
            
    //        Dispatcher.Dispatch(() =>
    //        {
    //            LocationService locationService = new LocationService();
    //            locationService.GetCurrentLocation();
    //            bool _isCheckingLocation = false;
    //            CancellationTokenSource _cancelTokenSource;
    //            Location location;

    //            try
    //            {
    //                _isCheckingLocation = true;

    //                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

    //                _cancelTokenSource = new CancellationTokenSource();

    //                location = Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token).Result;
    //                //Console.WriteLine("Longitude: " + location.Longitude + " Latitude: " + location.Latitude);
    //                //return null;
    //                if (location != null)
    //                {
    //                    Position.Text = $"({location.Longitude}, {location.Latitude})";
    //                    SemanticScreenReader.Announce(Position.Text);
    //                }
    //                //Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    
    //            }
    //            // Catch one of the following exceptions:
    //            //   FeatureNotSupportedException
    //            //   FeatureNotEnabledException
    //            //   PermissionException
    //            catch (Exception ex)
    //            {
    //                // Unable to get location
    //                Console.WriteLine("error: " + ex.Message);
    //            }
    //            finally
    //            {
    //                _isCheckingLocation = false;
    //            }
    //            //location = service.GetCurrentLocation();
    //            //Position.Text = $"({location.Longitude}, {location.Latitude})";
    //            //SemanticScreenReader.Announce(Position.Text);
    //        }) ; 

    //    }
    //    catch (NullReferenceException)
    //    {

    //        Console.WriteLine("Null error");
    //    }
        
    //}

    private async void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
    {
        time = (int)e.NewValue;

        Interval.Text = $"{(int)e.NewValue} Seconds";
        SemanticScreenReader.Announce(Interval.Text);

        //location = service.GetCurrentLocation();
        //Position.Text = $"({location.Longitude}, {location.Latitude})";
        //SemanticScreenReader.Announce(Position.Text);
    }

    private async void OnSetTimeIntervalClicked(object sender, EventArgs e)
    {
        var permissions = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

        if (permissions == PermissionStatus.Granted)
        {
            await ShareLocation();
        }
        else
        {
            await App.Current.MainPage.DisplayAlert("Permissions Error", "You have not granted the app permission to access your location.", "OK");

            var requested = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

            if (requested == PermissionStatus.Granted)
            {
                await ShareLocation();
                Position.Text = $"({location.Longitude}, {location.Latitude})";
                SemanticScreenReader.Announce(Position.Text);
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Location Required", "Location is required to share it. We'll ask again next time.", "OK");
            }
        }   
    }

    private async Task ShareLocation()
    {

        var locationRequest = new GeolocationRequest(GeolocationAccuracy.Best);
        var location = await Geolocation.GetLocationAsync(locationRequest);

        Position.Text = $"({location.Longitude}, {location.Latitude})";
        SemanticScreenReader.Announce(Position.Text);

        Console.WriteLine($"{location.Latitude} : {location.Longitude}");
    }

}

