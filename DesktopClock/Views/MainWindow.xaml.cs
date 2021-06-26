using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using DesktopClock.Resource;
using DryIoc;
using ModernWpf.Controls;

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
                Left = WpfScreenHelper.Screen.PrimaryScreen.WorkingArea.Left;
                Top  = WpfScreenHelper.Screen.PrimaryScreen.WorkingArea.Top;
            }
            Dispatcher.Invoke(() =>
                              {
                                  Weather.Text           = "Loading Weather...";
                                  WeatherInfo.Foreground = new SolidColorBrush(Colors.White);
                              });

            GlobalVariable.GlobalContainer.RegisterInstance(this);
            GlobalVariable.WindowStarted.Set();
        }

        private void window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
                MyConfig.Default.LastLeft = Left;
                MyConfig.Default.LastTop  = Top;
                MyConfig.Default.Save();
            }
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
