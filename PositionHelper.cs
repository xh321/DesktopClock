using System;
using System.Device.Location;

namespace DesktopTimer
{
    public delegate void OnPositionChangedEventHandle(object sender, PositionChangedEventArgs e);
    public delegate void OnAddressResolveredEventHandle(object sender, AddressResolverEventArgs e);
    public class AddressResolverEventArgs : PositionChangedEventArgs
    {
        /// <summary>
        /// 地址1
        /// </summary>
        public string Address1 { get; set; }
        /// <summary>
        /// 地址2
        /// </summary>
        public string Address2 { get; set; }
        /// <summary>
        /// 地址3
        /// </summary>
        public string Address3 { get; set; }
        public AddressResolverEventArgs()
        {

        }
    }
    public class PositionChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 经度
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public double Latitude { get; set; }

        public object Tag { get; set; }

        public PositionChangedEventArgs()
        {

        }
    }
    public class DevicePositioning
    {
        private CivicAddressResolver _address = null;
        private GeoCoordinateWatcher _location = null;
        private GeoCoordinate _lastPosition = GeoCoordinate.Unknown;
        private volatile bool _locationOn = true;
        private bool _resolverByPositionChanged = true;

        public event OnAddressResolveredEventHandle OnAddressResolvered;

        /// <summary>
        /// 当前位置
        /// </summary>
        public GeoCoordinate Position
        {
            get { return _lastPosition; }
        }


        public DevicePositioning()
        {
            _location = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
            //
            _location.MovementThreshold = 1.0;//1米
            _location.PositionChanged += Location_PositionChanged;
            //
            _address = new CivicAddressResolver();
            _address.ResolveAddressCompleted += Address_ResolveAddressCompleted;
        }
        /// <summary>
        /// 异步定位取Position值
        /// </summary>
        public void Positioning()
        {
            bool started = false;
            _resolverByPositionChanged = _locationOn = true;
            try
            {
                started = _location.TryStart(true, TimeSpan.FromMilliseconds(1024));
                //_location.TryStart(
                //_location.Start(true);
                //
                if (started)
                {
                    //if (_location.Position.Location.IsUnknown == false)
                    //{
                    //    _address.ResolveAddressAsync(_location.Position.Location);
                    //}
                }
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                if (!started && _locationOn)
                {
                    System.Threading.Thread.Sleep(128);
                    Positioning();
                }
            }
        }
        public void UnPositioning()
        {
            try
            {
                _locationOn = false;
                if (_location != null)
                    _location.Stop();
            }
            catch (Exception ex)
            {
                
            }
        }

        public void AddressResolver(double lat, double lon)
        {
            InnerAddressResolver(new GeoCoordinate(lat, lon));
        }
        private void InnerAddressResolver(GeoCoordinate position)
        {
            try
            {
                _lastPosition = position;
                _address.ResolveAddressAsync(_lastPosition);
            }
            catch (Exception ex)
            {
                
            }
        }
        private void Location_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            try
            {
                _lastPosition = e.Position.Location;
                if (!_lastPosition.IsUnknown && _resolverByPositionChanged)
                {
                    _address.ResolveAddressAsync(_lastPosition);
                }
            }
            catch (Exception ex)
            {
               
            }
            finally
            {
                _resolverByPositionChanged = false;
            }
        }
        private void Address_ResolveAddressCompleted(object sender, ResolveAddressCompletedEventArgs e)
        {
            try
            {
                string address = string.Empty;
                if (e.Address.IsUnknown)
                {
                    address = "Unknown [" + _lastPosition.Longitude + "," + _lastPosition.Latitude + "] Address.";
                }
                else
                {
                    //address = e.Address.AddressLine1;
                    //address = e.Address.AddressLine2;
                    address =
                        e.Address.CountryRegion +
                        e.Address.StateProvince +
                        e.Address.City +
                        e.Address.Building +
                        e.Address.FloorLevel;
                }
                if (OnAddressResolvered != null)
                {
                    OnAddressResolvered.BeginInvoke(this, new AddressResolverEventArgs()
                    {
                        Longitude = _lastPosition.Longitude,
                        Latitude = _lastPosition.Latitude,
                        Address1 = e.Address.AddressLine1,
                        Address2 = e.Address.AddressLine2,
                        Address3 = address
                    }, End_CallBack, null);
                }
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
            }
        }

        private void End_CallBack(IAsyncResult ar)
        {
            try
            {
                if (ar.IsCompleted)
                {
                    if (OnAddressResolvered != null)
                        OnAddressResolvered.EndInvoke(ar);
                }
            }
            catch (Exception ex)
            {
               
            }
        }
    }
}
