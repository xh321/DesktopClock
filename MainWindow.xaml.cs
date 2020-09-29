using Microsoft.Win32;
using System;
using System.Drawing;
using System.Globalization;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Color = System.Windows.Media.Color;

namespace DesktopTimer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Timer  timerThread;
        public static Timer  weatherTimer;
        DevicePositioning    dp = new DevicePositioning();
        public static string currLon;
        public static string currLat;

        class WeatherStatus
        {
            public string WeatherIco   = "ERROR LOADING WEATHER";
            public string Temp         = "";
            public string Wind         = "";
            public Color  WeatherColor = Colors.White;
            public Color  TempColor    = Colors.White;
            public Color  WindColor    = Colors.White;
        }

        public MainWindow()
        {
            InitializeComponent();
            //if (MyConfig.Default.LastLeft < 0) MyConfig.Default.LastLeft = 0;
            //if (MyConfig.Default.LastTop  < 0) MyConfig.Default.LastTop  = 0;
            //MyConfig.Default.Save();
            if (MyConfig.Default.LastLeft != 0)
            {
                Left = MyConfig.Default.LastLeft;
                Top  = MyConfig.Default.LastTop;
            }
            else
            {
                Left = WpfScreenHelper.Screen.PrimaryScreen.WorkingArea.Left;
                Top  = WpfScreenHelper.Screen.PrimaryScreen.WorkingArea.Top;
            }

            Dispatcher.Invoke(() =>
                              {
                                  WeatherIcon.Text       = "Loading Weather...";
                                  WeatherInfo.Foreground = new SolidColorBrush(Colors.White);
                              });
            dp.Positioning();
            dp.OnAddressResolvered += Dp_OnAddressResolved;

            timerThread = new Timer(Timer_Tick, null, new TimeSpan(0), new TimeSpan(0, 0, 0, 0, 500));
        }

        private void Dp_OnAddressResolved(object sender, AddressResolverEventArgs e)
        {
            currLon = Math.Round(e.Latitude, 5).ToString();
            currLat = Math.Round(e.Longitude, 5).ToString();

            RefreshWeather(null);
            weatherTimer = new Timer(RefreshWeather, null, new TimeSpan(0), new TimeSpan(1, 0, 0));
        }

        private void RefreshWeather(object obj)
        {
            Thread Update = new Thread(() =>
                                       {
                                           Dispatcher.Invoke(() =>
                                                             {
                                                                 WeatherIcon.Text = "Refreshing...";
                                                                 WeatherInfo.Foreground =
                                                                     new SolidColorBrush(Colors.White);
                                                                 Temp.Text = "";
                                                                 Wind.Text = "";
                                                                 RotateTransform rtf = new RotateTransform();
                                                                 RefreshWeatherBtn.RenderTransform = rtf;
                                                                 //DoubleAnimation dbAscending = new DoubleAnimation(0, 360, new Duration(TimeSpan.FromSeconds(1)));
                                                                 DoubleAnimation ani = new DoubleAnimation();
                                                                 ani.From     = 0;
                                                                 ani.To       = 360;
                                                                 ani.Duration = TimeSpan.FromMilliseconds(500);
                                                                 Storyboard storyboard = new Storyboard();
                                                                 //ani.RepeatBehavior = RepeatBehavior.Forever;
                                                                 storyboard.Children.Add(ani);
                                                                 Storyboard.SetTarget(ani, RefreshWeatherBtn);
                                                                 Storyboard.SetTargetProperty(ani,
                                                                     new PropertyPath("RenderTransform.Angle"));
                                                                 storyboard.Begin();
                                                             });
                                           string    lat = currLat;
                                           string    lon = currLon;
                                           WebClient wc  = new WebClient();
                                           wc.Encoding = System.Text.Encoding.UTF8;
                                           // var cityInfo = (JObject)JsonConvert.DeserializeObject(wc.DownloadString($"https://geocode.xyz/{lat},{lon}?geoit=json"));
                                           var weatherStatus =
                                               ParseWeather(wc.DownloadString($"http://wttr.in/{lon},{lat}?format=2"));

                                           Dispatcher.Invoke(() =>
                                                             {
                                                                 WeatherIcon.Text = weatherStatus.WeatherIco;
                                                                 WeatherIcon.Foreground =
                                                                     new SolidColorBrush(weatherStatus.WeatherColor);
                                                                 Temp.Text = weatherStatus.Temp;
                                                                 Temp.Foreground =
                                                                     new SolidColorBrush(weatherStatus.TempColor);
                                                                 Wind.Text = weatherStatus.Wind;
                                                                 Wind.Foreground =
                                                                     new SolidColorBrush(weatherStatus.WindColor);
                                                             });
                                       });
            Update.Start();
        }


        private bool   LastStatus  = false;
        private bool   LastStatusM = false;
        private string lastMinute  = "";

        private void Timer_Tick(object sender)
        {
            Dispatcher.Invoke(() =>
                              {
                                  if (GetThemeStyle() == "1") //亮色
                                  {
                                      tHour_.Foreground  = new SolidColorBrush(GetColorFromHex("#36BBCE"));
                                      tMinute.Foreground = new SolidColorBrush(GetColorFromHex("#33CCCC"));
                                      tSecond.Foreground = new SolidColorBrush(GetColorFromHex("#5CCCCC"));
                                      tYear.Foreground =
                                          new LinearGradientBrush(GetColorFromHex("#BF7130"),
                                                                  GetColorFromHex("#FF7400"), 45.0);
                                      tDay.Foreground =
                                          new LinearGradientBrush(GetColorFromHex("#FF7400"),
                                                                  GetColorFromHex("#FFB273"), 45.0);
                                      HourMinuteDot.Foreground =
                                          new LinearGradientBrush(GetColorFromHex("#009999"),
                                                                  GetColorFromHex("#33CCCC"), 45.0);
                                      MinuteSecondDot.Foreground =
                                          new LinearGradientBrush(GetColorFromHex("#33CCCC"),
                                                                  GetColorFromHex("#5CCCCC"), 45.0);
                                  }
                                  else //暗色
                                  {
                                      Color color1 = GetColorFromHex("#057D9F");
                                      Color color2 = GetColorFromHex("#39AECF");
                                      Color color3 = GetColorFromHex("#61B7CF");
                                      Color color4 = GetColorFromHex("#5DC8CD");
                                      Color color5 = GetColorFromHex("#3F92D2");
                                      Color color6 = GetColorFromHex("#0B61A4");


                                      tHour_.Foreground = new LinearGradientBrush(color1, color2, 45.0);
                                      HourMinuteDot.Foreground =
                                          new LinearGradientBrush(color2, color3, 45.0);
                                      tMinute.Foreground = new LinearGradientBrush(color3, color4, 45.0);
                                      MinuteSecondDot.Foreground =
                                          new LinearGradientBrush(color4, color5, 45.0);
                                      tSecond.Foreground = new LinearGradientBrush(color5, color6, 45.0);

                                      tYear.Foreground =
                                          new LinearGradientBrush(GetColorFromHex("#1049A9"),
                                                                  GetColorFromHex("#87baf3"), 45.0);
                                      tDay.Foreground =
                                          new LinearGradientBrush(GetColorFromHex("#87baf3"),
                                                                  GetColorFromHex("#052C6E"), 45.0);
                                  }

                                  tHour_.Content          = DateTime.Now.Hour.ToString().PadLeft(2, '0');
                                  MinuteSecondDot.Content = (LastStatus = !LastStatus) ? ":" : "";
                                  if (lastMinute != DateTime.Now.Minute.ToString())
                                  {
                                      lastMinute  = DateTime.Now.Minute.ToString();
                                      LastStatusM = false;
                                  }
                                  else
                                  {
                                      LastStatusM = true;
                                  }

                                  HourMinuteDot.Content = (LastStatusM) ? ":" : "";
                                  tMinute.Content       = DateTime.Now.Minute.ToString().PadLeft(2, '0');
                                  tSecond.Content       = DateTime.Now.Second.ToString().PadLeft(2, '0');
                                  tYear.Content =
                                      DateTime.Now.ToString("yyyy-MM-dd",
                                                            CultureInfo.CreateSpecificCulture("en-GB"));
                                  tDay.Content =
                                      DateTime.Now.ToString("ddd",
                                                            CultureInfo.CreateSpecificCulture("en-GB"));
                              });
        }

        private WeatherStatus ParseWeather(string input)
        {
            input = input.Replace("  ", "").Trim();
            string weatherIco    = String_GetLeft(input, "🌡").Trim();
            string Temp          = GetBetween(input, "🌡", "🌬").Trim();
            string Wind          = String_GetRight_Last(input, "🌬").Trim();
            string WindDirection = "";
            //Emoji双字符，会遗留一个空白字符，需根据第二个字符是否为风向来判断
            if (!Char.IsDigit(Wind.ToCharArray()[1])) //如果不是数字，那就是风向了
            {
                WindDirection = Wind.Substring(1, 1); //风向
                Wind          = Wind.Substring(2);    //去掉风向之后的风速
            }

            var re = new WeatherStatus();
            re.WeatherIco = weatherIco + " ";
            switch (weatherIco)
            {
                case "☀️":
                    re.WeatherColor = Colors.Yellow;
                    break;
                case "⛅️":
                    re.WeatherColor = Colors.Orange;
                    break;
                case "🌦":
                    re.WeatherColor = Colors.Blue;
                    break;
                case "⛈️":
                    re.WeatherColor = Colors.Blue;
                    break;
                case "🌩":
                    re.WeatherColor = Colors.Yellow;
                    break;
                case "☁️":
                    re.WeatherColor = Colors.Gray;
                    break;
                case "🌧":
                    re.WeatherColor = Colors.DarkBlue;
                    break;
                case "🌁":
                    re.WeatherColor = Colors.Gray;
                    break;
                case "🌫":
                    re.WeatherColor = Colors.White;
                    break;
                case "❄️":
                    re.WeatherColor = Colors.White;
                    break;
                case "🌨":
                    re.WeatherColor = Colors.White;
                    break;
                default:
                    re.WeatherColor = Colors.White;
                    break;
            }

            re.Temp = "🌡" + Temp + " ";       //SubString从第二个取的原因是Emoji字符会遗留一个空白字符
            if (!int.TryParse(String_GetLeft(Temp.Substring(1), "°C"), out int temp))
            {
                temp = 0;
            }

            if (temp < -20)
            {
                re.TempColor = Colors.DarkBlue;
            }

            if (temp < -10)
            {
                re.TempColor = Colors.Blue;
            }
            else if (temp < 10)
            {
                re.TempColor = Colors.DarkTurquoise;
            }
            else if (temp < 20)
            {
                re.TempColor = Colors.Aqua;
            }
            else if (temp < 30)
            {
                re.TempColor = Colors.Yellow;
            }
            else if (temp < 40)
            {
                re.TempColor = Colors.Orange;
            }
            else if (temp < 50)
            {
                re.TempColor = Colors.OrangeRed;
            }
            else if (temp < 60)
            {
                re.TempColor = Colors.Red;
            }
            else
            {
                re.TempColor = Colors.DarkRed;
            }

            re.Wind = "🌬" + WindDirection + Wind + " ";
            if (!int.TryParse(String_GetLeft(Wind, "km/h"), out int wind))
            {
                wind = 0;
            }

            if (wind < 5)
            {
                re.WindColor = Colors.White;
            }
            else if (wind < 10)
            {
                re.WindColor = Colors.Aquamarine;
            }
            else if (wind < 15)
            {
                re.WindColor = Colors.SkyBlue;
            }
            else if (wind < 20)
            {
                re.WindColor = Colors.Aqua;
            }
            else if (wind < 25)
            {
                re.WindColor = Colors.DeepSkyBlue;
            }
            else if (wind < 30)
            {
                re.WindColor = Colors.Blue;
            }
            else if (wind < 35)
            {
                re.WindColor = Colors.MediumBlue;
            }
            else
            {
                re.WindColor = Colors.DarkBlue;
            }

            return re;
        }

        private System.Windows.Media.Color GetColorFromHex(string hex)
        {
            var color   = ColorTranslator.FromHtml(hex);
            var toColor = new System.Windows.Media.Color();
            toColor.R = color.R;
            toColor.G = color.G;
            toColor.B = color.B;
            toColor.A = color.A;
            return toColor;
        }

        private String GetThemeStyle()
        {
            return Registry.CurrentUser
                           .OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize", false)
                           .GetValue("SystemUsesLightTheme").ToString();
        }

        public static string GetBetween(string text, string left, string right)
        {
            //判断是否为null或者是empty
            if (string.IsNullOrEmpty(left))
                return "";
            if (string.IsNullOrEmpty(right))
                return "";
            if (string.IsNullOrEmpty(text))
                return "";
            //判断是否为null或者是empty

            int Lindex = text.IndexOf(left); //搜索left的位置

            if (Lindex == -1)
            {
                //判断是否找到left
                return "";
            }

            Lindex = Lindex + left.Length; //取出left右边文本起始位置

            int Rindex = text.IndexOf(right, Lindex); //从left的右边开始寻找right

            if (Rindex == -1)
            {
                //判断是否找到right
                return "";
            }

            return text.Substring(Lindex, Rindex - Lindex); //返回查找到的文本
        }


        static public string String_GetLeft(string in_str, string find_str)
        {
            string re_1  = "";
            int    index = in_str.IndexOf(find_str);
            if (index > 0)
            {
                re_1 = in_str.Substring(0, index);
            }

            return re_1;
        }


        static public string String_GetRight_Last(string in_str, string find_str)
        {
            string[] sz;

            sz = in_str.Split(find_str.ToCharArray());
            string re = "";
            if (true)
            {
                if (sz.Length > 0)
                {
                    re = sz[sz.Length - 1];
                }
            }

            return re;
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
        }

        private void window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
                MyConfig.Default.LastLeft = this.Left;
                MyConfig.Default.LastTop  = this.Top;
                MyConfig.Default.Save();
            }
        }

        private void RefreshWeatherBtn_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            RefreshWeather(null);
        }
    }
}