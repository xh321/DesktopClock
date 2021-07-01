using System;
using System.Windows.Media;
using DesktopClock.Extensions;

namespace DesktopClock.Utils
{
    public static class WeatherUtils
    {
        public class WeatherStatus
        {
            public string WeatherIco = "ERROR LOADING WEATHER";
            public string Temp = "";
            public string Wind = "";
            public string WindLevel = "";
            public Color WeatherColor = Colors.White;
            public Color TempColor = Colors.White;
            public Color WindColor = Colors.White;
        }

        private static Color WeatherColorSwitcher(string icon) =>
            icon switch
            {
                "☀️" => Colors.Yellow,
                "⛅️" => Colors.Orange,
                "🌦" => Colors.Blue,
                "⛈️" => Colors.Blue,
                "🌩" => Colors.Yellow,
                "☁️" => Colors.Gray,
                "🌧" => Colors.DarkBlue,
                "🌁" => Colors.Gray,
                "🌫" => Colors.White,
                "❄️" => Colors.White,
                "🌨" => Colors.White,
                _ => Colors.White,
            };

        private static Color TempColorSwitcher(int temp) =>
            temp switch
            {
                < -20 => Colors.DarkBlue,
                < -10 => Colors.MidnightBlue,
                < 10 => Colors.Blue,
                < 20 => Colors.DarkTurquoise,
                < 30 => Colors.Aqua,
                < 35 => Colors.Yellow,
                < 40 => Colors.Orange,
                < 50 => Colors.OrangeRed,
                < 60 => Colors.Red,
                _ => Colors.DarkRed,
            };

        private static (string, Color) WindConvertor(int wind)
        {
            // calculate wind level via magic.
            var windLevel = (int)(0.0979 * wind + 0.317 + 0.5);

            // calculate wind color.
            Color windColor = windLevel >= 12
                ? Colors.Navy
                : new()
                {
                    A = 255,
                    R = (byte)(240 - 18 * windLevel),
                    G = (byte)(240 - 18 * windLevel),
                    B = (byte)(255 - 12 * windLevel)
                };

            return (windLevel.ToString(), windColor);
        }

        [Obsolete]
        private static (string, Color) WindSwitcher(int wind) =>
             wind switch
             {
                 < 1 => ("0", Colors.White),
                 <= 5 => ("1", Colors.AliceBlue),
                 <= 19 => ("2", Colors.Aquamarine),
                 <= 28 => ("3", Colors.Cyan),
                 <= 38 => ("4", Colors.Cyan),
                 <= 49 => ("5", Colors.DeepSkyBlue),
                 <= 61 => ("6", Colors.CornflowerBlue),
                 <= 74 => ("7", Colors.DodgerBlue),
                 <= 88 => ("8", Colors.Blue),
                 <= 102 => ("9", Colors.MediumBlue),
                 <= 117 => ("10", Colors.DarkBlue),
                 <= 134 => ("11", Colors.MidnightBlue),
                 <= 149 => ("12", Colors.Navy),
                 <= 166 => ("13", Colors.Navy),
                 <= 183 => ("14", Colors.Navy),
                 <= 201 => ("15", Colors.Navy),
                 <= 220 => ("16", Colors.Navy),
                 _ => ("17", Colors.Navy)
             };


        public static WeatherStatus ParseWeather(string input)
        {
            input = input.Trim();

            var weatherIco = input.GetLeft("🌡").Trim();
            var tempInput = input.GetBetween("🌡", "🌬").Trim();
            var windInput = input.GetRightLast("🌬").Trim();
            var windDirection = string.Empty;

            //Emoji双字符，会遗留一个空白字符，需根据第二个字符是否为风向来判断
            if (!char.IsDigit(windInput[1]))    //如果不是数字，那就是风向了
            {
                windDirection = windInput.Substring(1, 1);  //风向
                windInput = windInput[2..];                 //去掉风向之后的风速
            }

            _ = int.TryParse(tempInput[1..].GetLeft("°C"), out int temp);
            _ = int.TryParse(windInput.GetLeft("km/h"), out var wind);

            var ret = new WeatherStatus
            {
                WeatherIco = $"{weatherIco} ",
                WeatherColor = WeatherColorSwitcher(weatherIco),
                Temp = $"🌡{tempInput} ",
                TempColor = TempColorSwitcher(temp),
                Wind = $"💨{windDirection}{windInput} ",
            };

            (ret.WindLevel, ret.WindColor) = WindConvertor(wind);

            return ret;
        }
    }
}