using System;
using System.Globalization;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using DesktopClock.Resource;
using DesktopClock.Utils;
using DesktopClock.Views;
using DryIoc;
using Prism.Commands;
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
        private string _tempText            = "N/A";
        private string _windText            = "N/A";

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

        public string TempText
        {
            get => _tempText;
            set => SetProperty(ref _tempText, value);
        }

        public string WindText
        {
            get => _windText;
            set => SetProperty(ref _windText, value);
        }

        #endregion

        #region 事件绑定

        public DelegateCommand WeatherFreshButtonCommand => new(WeatherFreshCommand);

        #endregion

        #region 初始化

        public MainWindowViewModel()
        {
            Task.Run(() =>
                     {
                         //等待窗口完成启动
                         GlobalVariable.WindowStarted.WaitOne();
                         _clockTimer = new Timer(TimerTick, null, new TimeSpan(0),
                                                 new TimeSpan(0, 0, 0, 0, 500));
                         _weatherTimer = new Timer(RefreshWeather, null, new TimeSpan(0),
                                                   new TimeSpan(1, 0, 0));
                     });
        }

        #endregion

        #region 计时器事件处理

        /// <summary>
        /// 时钟tick
        /// </summary>
        /// <param name="obj"></param>
        private void TimerTick(object obj)
        {
            var mainWindow = GlobalVariable.GlobalContainer.Resolve<MainWindow>();
            var theme      = WindowsUtils.GetThemeStyle();
            if (theme != _lastTheme)
            {
                //先咕了
                switch (theme)
                {
                    case "1":
                        mainWindow.Dispatcher.Invoke(() =>
                                                     {
                                                         mainWindow.tHour_.Foreground =
                                                             new SolidColorBrush("#36BBCE".ToColor());
                                                         mainWindow.tMinute.Foreground =
                                                             new SolidColorBrush("#33CCCC".ToColor());
                                                         mainWindow.tSecond.Foreground =
                                                             new SolidColorBrush("#5CCCCC".ToColor());
                                                         mainWindow.tDate.Foreground =
                                                             new
                                                                 LinearGradientBrush("#BF7130".ToColor(),
                                                                    "#FF7400".ToColor(),
                                                                     45.0);
                                                         mainWindow.tDay.Foreground =
                                                             new
                                                                 LinearGradientBrush("#FF7400".ToColor(),
                                                                     "#FFB273".ToColor(),
                                                                     45.0);
                                                         mainWindow.HourMinuteDot.Foreground =
                                                             new
                                                                 LinearGradientBrush("#009999".ToColor(),
                                                                     "#33CCCC".ToColor(),
                                                                     45.0);
                                                         mainWindow.MinuteSecondDot.Foreground =
                                                             new
                                                                 LinearGradientBrush("#33CCCC".ToColor(),
                                                                     "#5CCCCC".ToColor(),
                                                                     45.0);
                                                     });
                        break;
                    case "0":
                        mainWindow.Dispatcher.Invoke(() =>
                                                     {
                                                         var color1 = "#057D9F".ToColor();
                                                         var color2 = "#39AECF".ToColor();
                                                         var color3 = "#61B7CF".ToColor();
                                                         var color4 = "#5DC8CD".ToColor();
                                                         var color5 = "#3F92D2".ToColor();
                                                         var color6 = "#0B61A4".ToColor();


                                                         mainWindow.tHour_.Foreground =
                                                             new LinearGradientBrush(color1, color2, 45.0);
                                                         mainWindow.HourMinuteDot.Foreground =
                                                             new LinearGradientBrush(color2, color3, 45.0);
                                                         mainWindow.tMinute.Foreground =
                                                             new LinearGradientBrush(color3, color4, 45.0);
                                                         mainWindow.MinuteSecondDot.Foreground =
                                                             new LinearGradientBrush(color4, color5, 45.0);
                                                         mainWindow.tSecond.Foreground =
                                                             new LinearGradientBrush(color5, color6, 45.0);

                                                         mainWindow.tDate.Foreground =
                                                             new
                                                                 LinearGradientBrush("#1049A9".ToColor(),
                                                                     "#87baf3".ToColor(),
                                                                     45.0);
                                                         mainWindow.tDay.Foreground =
                                                             new
                                                                 LinearGradientBrush("#87baf3".ToColor(),
                                                                     "#052C6E".ToColor(),
                                                                     45.0);
                                                     });
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
            var mainWindow = GlobalVariable.GlobalContainer.Resolve<MainWindow>();
            Task.Run(() => { mainWindow.RefreshingWeatherUI(); });

            WeatherText = "Refreshing...";
            TempText    = "";
            WindText    = "";

            //定位
            var location = Geolocation.GetLocation();

            var wc = new WebClient
            {
                Encoding = System.Text.Encoding.UTF8
            };
            string apiRet = "";
            try
            {
                apiRet = wc.DownloadString($"http://wttr.in/{location.Longitude},{location.Latitude}?format=2");
            }
            catch
            {
                // ignored
            }

            if (apiRet.Contains("Unknow"))
            {
                WeatherText = "Weather Service Down";
                mainWindow.Dispatcher.Invoke(() =>
                                             {
                                                 mainWindow.WeatherInfo.Foreground =
                                                     new SolidColorBrush(Colors.White);
                                             });
            }
            else if (string.IsNullOrEmpty(apiRet))
            {
                WeatherText = "Network Connect Error";
                mainWindow.Dispatcher.Invoke(() =>
                                             {
                                                 mainWindow.WeatherInfo.Foreground =
                                                     new SolidColorBrush(Colors.White);
                                             });
            }
            else
            {
                var weatherStatus = WeatherUtils.ParseWeather(apiRet);
                mainWindow.Dispatcher.Invoke(() =>
                                             {
                                                 mainWindow.Weather.Foreground =
                                                     new SolidColorBrush(weatherStatus.WeatherColor);
                                                 mainWindow.Temp.Foreground =
                                                     new SolidColorBrush(weatherStatus.TempColor);
                                                 mainWindow.Wind.Foreground =
                                                     new SolidColorBrush(weatherStatus.WindColor);
                                             });
                WeatherText =  weatherStatus.WeatherIco;
                TempText    =  weatherStatus.Temp;
                WindText    =  weatherStatus.Wind;
                WindText    += "   LEVEL  " + weatherStatus.WindLevel;
            }
        }

        #endregion

        #region UI事件

        public void WeatherFreshCommand()
        {
            RefreshWeather(null);
        }

        #endregion
    }
}