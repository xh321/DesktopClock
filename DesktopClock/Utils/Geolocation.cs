using System;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace DesktopClock.Utils
{
    public static class Geolocation
    {
        public static async Task<BasicGeoposition> GetLocation()
        {
            var locator  = new Windows.Devices.Geolocation.Geolocator();
            var location = await locator.GetGeopositionAsync();
            var position = location.Coordinate.Point.Position;
            return position;
        }
    }
}