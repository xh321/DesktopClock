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

        //TODO 草这是什么我留个todo你自己看了（
        private static (string, Color) WindConvertor(int wind)
        {
            var windLevel = ((int) (0.0979 * wind + 0.317 + 0.5)).ToString();
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
            input = input.Replace("  ", "").Trim();

            var weatherIco    = StringUtils.String_GetLeft(input, "🌡").Trim();
            var tempInput     = StringUtils.GetBetween(input, "🌡", "🌬").Trim();
            var windInput     = StringUtils.String_GetRight_Last(input, "🌬").Trim();
            var windDirection = "";

            //Emoji双字符，会遗留一个空白字符，需根据第二个字符是否为风向来判断

            if (!char.IsDigit(windInput.ToCharArray()[1])) //如果不是数字，那就是风向了
            {
                windDirection = windInput.Substring(1, 1); //风向
                windInput     = windInput[2..];            //去掉风向之后的风速
            }

            _ = int.TryParse(StringUtils.String_GetLeft(tempInput[1..], "°C"), out int temp);
            _ = int.TryParse(StringUtils.String_GetLeft(windInput, "km/h"), out var wind);

            var ret = new WeatherStatus
            {
                WeatherIco   = weatherIco + " ",
                WeatherColor = WeatherColorSwitcher(weatherIco),
                Temp         = "🌡" + tempInput + " ", //SubString从第二个取的原因是Emoji字符会遗留一个空白字符
                TempColor    = TempColorSwitcher(temp),
                Wind         = "💨" + windDirection + windInput + " ",
            };

            (ret.WindLevel, ret.WindColor) = WindSwitcher(wind);

            return ret;
        }
    }
}