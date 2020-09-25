using Microsoft.Win32;
using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Color = System.Windows.Media.Color;

namespace DesktopTimer
{

    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Timer timerThread;
        public MainWindow()
        {
            InitializeComponent();
            var screen = WpfScreenHelper.Screen.AllScreens.OrderBy(i=>i.DeviceName).ToList()[0];
            Left = screen.Bounds.Left + Width - 420;
            Top  = screen.Bounds.Top;

            timerThread = new Timer(Timer_Tick, null, new TimeSpan(0), new TimeSpan(0, 0, 1));
        }

        private bool   LastStatus  = false;
        private bool   LastStatusM = false;
        private string lastMinute  = "";

        private void Timer_Tick(object sender)
        {

          
            Dispatcher.Invoke(new Action(() =>
                                         {
                                             if (GetThemeStyle() == "1")//亮色
                                             {
                                                 tHour_.Foreground = new SolidColorBrush(GetColorFromHex("#36BBCE"));
                                                 tMinute.Foreground = new SolidColorBrush(GetColorFromHex("#33CCCC"));
                                                 tSecond.Foreground = new SolidColorBrush(GetColorFromHex("#5CCCCC"));
                                                 tYear.Foreground = new LinearGradientBrush(GetColorFromHex("#BF7130"),GetColorFromHex("#FF7400"),45.0);
                                                 tDay.Foreground = new LinearGradientBrush(GetColorFromHex("#FF7400"), GetColorFromHex("#FFB273"), 45.0);
                                                 HourMinuteDot.Foreground = new LinearGradientBrush(GetColorFromHex("#009999"), GetColorFromHex("#33CCCC"), 45.0);
                                                 MinuteSecondDot.Foreground = new LinearGradientBrush(GetColorFromHex("#33CCCC"), GetColorFromHex("#5CCCCC"), 45.0);

                                             }
                                             else//暗色
                                             {
                                                 Color color1 = GetColorFromHex("#057D9F");
                                                 Color color2 = GetColorFromHex("#39AECF");
                                                 Color color3 = GetColorFromHex("#61B7CF");
                                                 Color color4 = GetColorFromHex("#5DC8CD");
                                                 Color color5 = GetColorFromHex("#3F92D2");
                                                 Color color6 = GetColorFromHex("#0B61A4");


                                                 tHour_.Foreground = new LinearGradientBrush(color1, color2, 45.0);
                                                 HourMinuteDot.Foreground = new LinearGradientBrush(color2, color3, 45.0);
                                                 tMinute.Foreground = new LinearGradientBrush(color3, color4,45.0);
                                                 MinuteSecondDot.Foreground = new LinearGradientBrush(color4, color5, 45.0);
                                                 tSecond.Foreground = new LinearGradientBrush(color5, color6, 45.0);
                                                 
                                                 tYear.Foreground = new LinearGradientBrush(GetColorFromHex("#1049A9"),GetColorFromHex("#87baf3"),45.0);
                                                 tDay.Foreground = new LinearGradientBrush(GetColorFromHex("#87baf3"), GetColorFromHex("#052C6E"),45.0);
                                                 
                                                 
                                                 
                                             }
                                             tHour_.Content        = DateTime.Now.Hour.ToString().PadLeft(2, '0');
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
                                             tMinute.Content         = DateTime.Now.Minute.ToString().PadLeft(2, '0');
                                             tSecond.Content         = DateTime.Now.Second.ToString().PadLeft(2, '0');
                                             tYear.Content =
                                                 DateTime.Now.ToString("yyyy-MM-dd",
                                                                       CultureInfo.CreateSpecificCulture("en-GB"));
                                             tDay.Content =
                                                 DateTime.Now.ToString("ddd",
                                                                       CultureInfo.CreateSpecificCulture("en-GB"));
                                         }));
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
            return Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize", false).GetValue("SystemUsesLightTheme").ToString();
        }
        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}