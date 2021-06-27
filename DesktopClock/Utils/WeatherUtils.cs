using System;
using System.Windows.Media;

namespace DesktopClock.Utils
{
    public static class WeatherUtils
    {
        public class WeatherStatus
        {
            public string WeatherIco   = "ERROR LOADING WEATHER";
            public string Temp         = "";
            public string Wind         = "";
            public string WindLevel    = "";
            public Color  WeatherColor = Colors.White;
            public Color  TempColor    = Colors.White;
            public Color  WindColor    = Colors.White;
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
                _ when temp < -20 => Colors.DarkBlue,
                _ when temp < -10 => Colors.MidnightBlue,
                _ when temp < 10 => Colors.Blue,
                _ when temp < 20 => Colors.DarkTurquoise,
                _ when temp < 30 => Colors.Aqua,
                _ when temp < 35 => Colors.Yellow,
                _ when temp < 40 => Colors.Orange,
                _ when temp < 50 => Colors.OrangeRed,
                _ when temp < 60 => Colors.Red,
                _ => Colors.DarkRed,
            };

        private static (string, Color) WindConvertor(int wind)
        {

            var windLevel = ((int)(0.0979 * wind + 0.317 + 0.5)).ToString();
            var windColor = new Color
            {

            };

            // FFFFFFFF
            // FFF0F8FF
            // FF7FFFD4
            // FF00FFFF
            // FF00FFFF
            // FF00BFFF
            // FF6495ED
            // FF1E90FF
            // FF0000FF
            // FF0000CD
            // FF00008B
            // FF191970
            // FF000080
            throw new NotImplementedException();
        }


        private static (string, Color) WindSwitcher(int wind) =>
            wind switch
            {
                _ when wind < 1 => ("0", Colors.White),
                _ when wind <= 5 => ("1", Colors.AliceBlue),
                _ when wind <= 19 => ("2", Colors.Aquamarine),
                _ when wind <= 28 => ("3", Colors.Cyan),
                _ when wind <= 38 => ("4", Colors.Cyan),
                _ when wind <= 49 => ("5", Colors.DeepSkyBlue),
                _ when wind <= 61 => ("6", Colors.CornflowerBlue),
                _ when wind <= 74 => ("7", Colors.DodgerBlue),
                _ when wind <= 88 => ("8", Colors.Blue),
                _ when wind <= 102 => ("9", Colors.MediumBlue),
                _ when wind <= 117 => ("10", Colors.DarkBlue),
                _ when wind <= 134 => ("11", Colors.MidnightBlue),
                _ when wind <= 149 => ("12", Colors.Navy),
                _ when wind <= 166 => ("13", Colors.Navy),
                _ when wind <= 183 => ("14", Colors.Navy),
                _ when wind <= 201 => ("15", Colors.Navy),
                _ when wind <= 220 => ("16", Colors.Navy),
                _ => ("17", Colors.Navy)
            };


        public static WeatherStatus ParseWeather(string input)
        {
            input = input.Replace("  ", "").Trim();

            var weatherIco = StringUtils.String_GetLeft(input, "🌡").Trim();
            var tempInput = StringUtils.GetBetween(input, "🌡", "🌬").Trim();
            var windInput = StringUtils.String_GetRight_Last(input, "🌬").Trim();
            var windDirection = "";

            //Emoji双字符，会遗留一个空白字符，需根据第二个字符是否为风向来判断

            if (!char.IsDigit(windInput.ToCharArray()[1])) //如果不是数字，那就是风向了
            {
                windDirection = windInput.Substring(1, 1); //风向
                windInput = windInput[2..];    //去掉风向之后的风速
            }

            _ = int.TryParse(StringUtils.String_GetLeft(tempInput[1..], "°C"), out int temp);
            _ = int.TryParse(StringUtils.String_GetLeft(windInput, "km/h"), out var wind);

            var ret = new WeatherStatus
            {
                WeatherIco = weatherIco + " ",
                WeatherColor = WeatherColorSwitcher(weatherIco),
                Temp = "🌡" + tempInput + " ",      //SubString从第二个取的原因是Emoji字符会遗留一个空白字符
                TempColor = TempColorSwitcher(temp),
                Wind = "💨" + windDirection + windInput + " ",
            };

            (ret.WindLevel, ret.WindColor) = WindSwitcher(wind);

            return ret;
        }
    }
}
