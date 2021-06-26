using System.Drawing;
using Microsoft.Win32;

namespace DesktopClock.Utils
{
    public static class WindowsUtils
    {
        public static string GetThemeStyle()
            => Registry.CurrentUser
                       .OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize", false)
                       ?.GetValue("AppsUseLightTheme")
                       ?.ToString();

        public static System.Windows.Media.Color GetColorFromHex(string hex)
        {
            var color   = ColorTranslator.FromHtml(hex);
            var toColor = new System.Windows.Media.Color
            {
                R = color.R,
                G = color.G,
                B = color.B,
                A = color.A
            };
            return toColor;
        }
    }
}
