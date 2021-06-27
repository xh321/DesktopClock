using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using DesktopClock.Resource;
using DesktopClock.Utils;
using DryIoc;
using WpfScreenHelper;

namespace DesktopClock.Views
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            if (MyConfig.Default.LastLeft != 0)
            {
                Left = MyConfig.Default.LastLeft;
                Top  = MyConfig.Default.LastTop;
            }
            else
            {
                Left = Screen.PrimaryScreen.WorkingArea.Left;
                Top  = Screen.PrimaryScreen.WorkingArea.Top;
            }

            Dispatcher.Invoke(() =>
                              {
                                  Weather.Text           = "Loading Weather...";
                                  WeatherInfo.Foreground = new SolidColorBrush(Colors.White);
                              });
            GlobalVariable.GlobalContainer.RegisterInstance(this);
            GlobalVariable.WindowStarted.Set();
        }

        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            ResizeWindow();
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
                MyConfig.Default.LastLeft = Left;
                MyConfig.Default.LastTop  = Top;
                MyConfig.Default.Save();
            }
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            ResizeWindow();
        }

        private void ResizeWindow()
        {
            Dispatcher.Invoke(() =>
                              {
                                  var screen =
                                      Screen.FromHandle(new WindowInteropHelper(this).Handle);
                                  ClockWindow.Height =
                                      330 * (screen.WorkingArea.Height / 1080);
                                  ClockWindow.Width =
                                      710 * (screen.WorkingArea.Width / 1920);
                                  MainViewBox.Height =
                                      330 * (screen.WorkingArea.Height / 1080);
                                  MainViewBox.Width =
                                      710 * (screen.WorkingArea.Width / 1920);
                              });
        }

        public void SwitchTheme(string targetTheme)
        {
            switch (targetTheme)
            {
                case "1":
                    Dispatcher.Invoke(() =>
                                      {
                                          tHour_.Foreground =
                                              new SolidColorBrush(WindowsUtils
                                                                      .GetColorFromHex("#36BBCE"));
                                          tMinute.Foreground =
                                              new SolidColorBrush(WindowsUtils
                                                                      .GetColorFromHex("#33CCCC"));
                                          tSecond.Foreground =
                                              new SolidColorBrush(WindowsUtils
                                                                      .GetColorFromHex("#5CCCCC"));
                                          tDate.Foreground =
                                              new
                                                  LinearGradientBrush(WindowsUtils.GetColorFromHex("#BF7130"),
                                                                      WindowsUtils.GetColorFromHex("#FF7400"),
                                                                      45.0);
                                          tDay.Foreground =
                                              new
                                                  LinearGradientBrush(WindowsUtils.GetColorFromHex("#FF7400"),
                                                                      WindowsUtils.GetColorFromHex("#FFB273"),
                                                                      45.0);
                                          HourMinuteDot.Foreground =
                                              new
                                                  LinearGradientBrush(WindowsUtils.GetColorFromHex("#009999"),
                                                                      WindowsUtils.GetColorFromHex("#33CCCC"),
                                                                      45.0);
                                          MinuteSecondDot.Foreground =
                                              new
                                                  LinearGradientBrush(WindowsUtils.GetColorFromHex("#33CCCC"),
                                                                      WindowsUtils.GetColorFromHex("#5CCCCC"),
                                                                      45.0);
                                      });
                    break;
                case "0":
                    Dispatcher.Invoke(() =>
                                      {
                                          Color color1 = WindowsUtils.GetColorFromHex("#057D9F");
                                          Color color2 = WindowsUtils.GetColorFromHex("#39AECF");
                                          Color color3 = WindowsUtils.GetColorFromHex("#61B7CF");
                                          Color color4 = WindowsUtils.GetColorFromHex("#5DC8CD");
                                          Color color5 = WindowsUtils.GetColorFromHex("#3F92D2");
                                          Color color6 = WindowsUtils.GetColorFromHex("#0B61A4");


                                          tHour_.Foreground =
                                              new LinearGradientBrush(color1, color2, 45.0);
                                          HourMinuteDot.Foreground =
                                              new LinearGradientBrush(color2, color3, 45.0);
                                          tMinute.Foreground =
                                              new LinearGradientBrush(color3, color4, 45.0);
                                          MinuteSecondDot.Foreground =
                                              new LinearGradientBrush(color4, color5, 45.0);
                                          tSecond.Foreground =
                                              new LinearGradientBrush(color5, color6, 45.0);

                                          tDate.Foreground =
                                              new
                                                  LinearGradientBrush(WindowsUtils.GetColorFromHex("#1049A9"),
                                                                      WindowsUtils.GetColorFromHex("#87baf3"),
                                                                      45.0);
                                          tDay.Foreground =
                                              new
                                                  LinearGradientBrush(WindowsUtils.GetColorFromHex("#87baf3"),
                                                                      WindowsUtils.GetColorFromHex("#052C6E"),
                                                                      45.0);
                                      });
                    break;
            }
        }

        public void ChangeInfoColor(Color color)
        {
            Dispatcher.Invoke(() => { WeatherInfo.Foreground = new SolidColorBrush(color); });
        }

        public void ChangeWeatherColor(Color weatherColor, Color tempColor, Color windColor)
        {
            Dispatcher.Invoke(() =>
                              {
                                  Weather.Foreground = new SolidColorBrush(weatherColor);
                                  Temp.Foreground    = new SolidColorBrush(tempColor);
                                  Wind.Foreground    = new SolidColorBrush(windColor);
                              });
        }

        public void RefreshingWeatherUI()
        {
            Dispatcher.Invoke(() =>
                              {
                                  WeatherInfo.Foreground =
                                      new SolidColorBrush(Colors.White);
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
        }
    }
}