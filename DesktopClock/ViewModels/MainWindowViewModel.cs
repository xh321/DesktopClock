using System;
using System.Globalization;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using DesktopClock.Resource;
using DesktopClock.Utils;
using DesktopClock.Views;
using DryIoc;
using Microsoft.Win32;
using Prism.Mvvm;

// ReSharper disable NotAccessedField.Local

namespace DesktopClock.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region 私有字段

        //UI
        private string _lastTheme           = string.Empty;
        private string _hourText            = "00";
        private string _minuteText          = "00";
        private string _secondText          = "00";
        private string _minuteSecondDotText = ":";
        private string _hourMinuteDotText   = ":";
        private string _dateText            = "0000-00-00";
        private string _dayText             = "N/A";
        private string _weatherText         = "N/A";
        private Brush  _weatherForeground   = new SolidColorBrush(Colors.White);
        private string _tempText            = "N/A";
        private Brush  _tempForeground      = new SolidColorBrush(Colors.Blue);
        private string _windText            = "N/A";
        private Brush  _windForeground      = new SolidColorBrush(Colors.Aqua);

        //状态
        private bool   LastStatus  = false;
        private bool   LastStatusM = false;
        private string lastMinute  = "";

        #endregion

        #region 计时器

        private static Timer _clockTimer;
        private static Timer _weatherTimer;

        #endregion

        #region 数据绑定

        public string HourText
        {
            get => _hourText;
            private set => SetProperty(ref _hourText, value);
        }

        public string MinuteText
        {
            get => _minuteText;
            private set => SetProperty(ref _minuteText, value);
        }

        public string SecondText
        {
            get => _secondText;
            private set => SetProperty(ref _secondText, value);
        }

        public string MinuteSecondDotText
        {
            get => _minuteSecondDotText;
            private set => SetProperty(ref _minuteSecondDotText, value);
        }

        public string HourMinuteDotText
        {
            get => _hourMinuteDotText;
            private set => SetProperty(ref _hourMinuteDotText, value);
        }

        public string DateText
        {
            get => _dateText;
            private set => SetProperty(ref _dateText, value);
        }

        public string DayText
        {
            get => _dayText;
            private set => SetProperty(ref _dayText, value);
        }

        public string WeatherText
        {
            get => _weatherText;
            set => SetProperty(ref _weatherText, value);
        }

        public Brush WeatherForeground
        {
            get => _weatherForeground;
            set => SetProperty(ref _weatherForeground, value);
        }

        public string TempText
        {
            get => _tempText;
            set => SetProperty(ref _tempText, value);
        }

        public Brush TempForeground
        {
            get => _tempForeground;
            set => SetProperty(ref _tempForeground, value);
        }

        public string WindText
        {
            get => _windText;
            set => SetProperty(ref _windText, value);
        }

        public Brush WindForeground
        {
            get => _windForeground;
            set => SetProperty(ref _windForeground, value);
        }

        #endregion

        #region 初始化

        public MainWindowViewModel()
        {
            _clockTimer   = new Timer(TimerTick, null, new TimeSpan(0), new TimeSpan(0, 0, 0, 0, 500));
            _weatherTimer = new Timer(RefreshWeather, null, TimeSpan.FromSeconds(5), new TimeSpan(1, 0, 0));
        }

        #endregion

        #region 计时器事件处理

        /// <summary>
        /// 时钟tick
        /// </summary>
        /// <param name="obj"></param>
        private void TimerTick(object obj)
        {
            var theme = WindowsUtils.GetThemeStyle();
            if (theme != _lastTheme)
            {
                //先咕了
                switch (theme)
                {
                    case "1":

                        break;
                    case "0":

                        break;
                }

                _lastTheme = theme;
            }

            HourText            = DateTime.Now.Hour.ToString().PadLeft(2, '0');
            LastStatus          = !LastStatus;
            MinuteSecondDotText = LastStatus ? ":" : "";

            if (lastMinute != DateTime.Now.Minute.ToString())
            {
                lastMinute  = DateTime.Now.Minute.ToString();
                LastStatusM = false;
            }
            else
            {
                LastStatusM = true;
            }

            HourMinuteDotText = LastStatusM ? ":" : "";
            MinuteText        = DateTime.Now.Minute.ToString().PadLeft(2, '0');
            SecondText        = DateTime.Now.Second.ToString().PadLeft(2, '0');

            DateText = DateTime.Now.ToString("yyyy-MM-dd",
                                             CultureInfo.CreateSpecificCulture("en-GB"));
            DayText = DateTime.Now.ToString("ddd",
                                            CultureInfo.CreateSpecificCulture("en-GB"));
        }

        /// <summary>
        /// 天气刷新
        /// </summary>
        /// <param name="obj"></param>
        private void RefreshWeather(object obj)
        {
            // Task.Run(() => { GlobalVariable.GlobalContainer.Resolve<MainWindow>().RefreshingWeatherUI(); });
            // WeatherText = "Refreshing...";
            // WeatherForeground =
            //     new SolidColorBrush(Colors.White);
            // TempText = "";
            // WindText = "";
            // //定位
            // var location = Geolocation.GetLocation();
            //
            // var wc = new WebClient
            // {
            //     Encoding = System.Text.Encoding.UTF8
            // };
            // string apiRet = "";
            // try
            // {
            //     apiRet = wc.DownloadString($"http://wttr.in/{location.Longitude},{location.Latitude}?format=2");
            // }
            // catch
            // {
            //     // ignored
            // }
            //
            // if (apiRet.Contains("Unknow"))
            // {
            //     WeatherText = "Weather Service Down";
            //     WeatherForeground =
            //         new SolidColorBrush(Colors.White);
            // }
            // else if (string.IsNullOrEmpty(apiRet))
            // {
            //     WeatherText       = "Network Connect Error";
            //     WeatherForeground = new SolidColorBrush(Colors.White);
            // }
            // else
            // {
            //     var weatherStatus = WeatherUtils.ParseWeather(apiRet);
            //     WeatherText = weatherStatus.WeatherIco;
            //     WeatherForeground =
            //         new SolidColorBrush(weatherStatus.WeatherColor);
            //     TempText = weatherStatus.Temp;
            //     TempForeground =
            //         new SolidColorBrush(weatherStatus.TempColor);
            //     WindText =  weatherStatus.Wind;
            //     WindText += "   LEVEL  " + weatherStatus.WindLevel;
            //     WindForeground =
            //         new SolidColorBrush(weatherStatus.WindColor);
            // }
        }

        #endregion
    }
}