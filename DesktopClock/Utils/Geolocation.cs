using System;
using Windows.Devices.Geolocation;

namespace DesktopClock.Utils
{
    public static class Geolocation
    {
        public static BasicGeoposition GetLocation()
        {
            var locator  = new Geolocator();
            var locationTask = locator.GetGeopositionAsync().AsTask();
            locationTask.Wait();
            var position = locationTask.Result.Coordinate.Point.Position;
            return position;
        }
    }
}