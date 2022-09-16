using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationSender.Services
{
    public class LocationService
    {
        private CancellationTokenSource _cancelTokenSource;
        private bool _isCheckingLocation;


        private static readonly HttpClient client = new HttpClient();
        private static readonly string route = "http://localhost:5000/sendLocation/";


        public async Task SendLocation()
        {
            Location location = this.GetCurrentLocation();

            var values = new Dictionary<string, string>
            {
                { "latitude", location.Latitude.ToString() },
                { "longitude", location.Longitude.ToString() },
                { "altitude", location.Altitude.ToString() }
            };

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync(route, content);

            var responseString = await response.Content.ReadAsStringAsync();
        }



        public Location GetCurrentLocation()
        {
            try
            {
                _isCheckingLocation = true;

                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

                _cancelTokenSource = new CancellationTokenSource();

                Location location = Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token).Result;
                //Console.WriteLine("Longitude: " + location.Longitude + " Latitude: " + location.Latitude);
                //return null;
                if (location != null)
                    return location;
                //Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                return null;
            }
            // Catch one of the following exceptions:
            //   FeatureNotSupportedException
            //   FeatureNotEnabledException
            //   PermissionException
            catch (Exception ex)
            {
                // Unable to get location
                //Console.WriteLine("error: " + ex.Message);
                return null;
            }
            finally
            {
                _isCheckingLocation = false;
            }
        }

        public void CancelRequest()
        {
            if (_isCheckingLocation && _cancelTokenSource != null && _cancelTokenSource.IsCancellationRequested == false)
                _cancelTokenSource.Cancel();
        }
    }
}
