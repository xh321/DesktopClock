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

        public static WeatherStatus ParseWeather(string input)
        {
            input = input.Replace("  ", "").Trim();
            string weatherIco    = StringUtils.String_GetLeft(input, "🌡").Trim();
            string Temp          = StringUtils.GetBetween(input, "🌡", "🌬").Trim();
            string Wind          = StringUtils.String_GetRight_Last(input, "🌬").Trim();
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
            if (!int.TryParse(StringUtils.String_GetLeft(Temp.Substring(1), "°C"), out int temp))
            {
                temp = 0;
            }

            if (temp < -20)
            {
                re.TempColor = Colors.DarkBlue;
            }

            if (temp < -10)
            {
                re.TempColor = Colors.MidnightBlue;
            }
            else if (temp < 10)
            {
                re.TempColor = Colors.Blue;
            }
            else if (temp < 20)
            {
                re.TempColor = Colors.DarkTurquoise;
            }
            else if (temp < 30)
            {
                re.TempColor = Colors.Aqua;
            }
            else if (temp < 35)
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

            re.Wind = "💨" + WindDirection + Wind + " ";
            if (!int.TryParse(StringUtils.String_GetLeft(Wind, "km/h"), out int wind))
            {
                wind = 0;
            }

            if(wind <1 )
            {
                re.WindLevel = "0";
                re.WindColor = Colors.White;
            }
            else if (wind <= 5)
            {
                re.WindLevel = "1";
                re.WindColor = Colors.AliceBlue;
            }
            else if (wind <= 19)
            {
                re.WindLevel = "2";
                re.WindColor = Colors.Aquamarine;
            }
            else if (wind <= 28)
            {
                re.WindLevel = "3";
                re.WindColor = Colors.Aqua;
            }
            else if (wind <= 38)
            {
                re.WindLevel = "4";
                re.WindColor = Colors.Cyan;
            }
            else if (wind <= 49)
            {
                re.WindLevel = "5";
                re.WindColor = Colors.DeepSkyBlue;
            }
            else if (wind <= 61)
            {
                re.WindLevel = "6";
                re.WindColor = Colors.CornflowerBlue;
            }
            else if (wind <= 74)
            {
                re.WindLevel = "7";
                re.WindColor = Colors.DodgerBlue;
            }
            else if (wind <= 88)
            {
                re.WindLevel = "8";
                re.WindColor = Colors.Blue;
            }
            else if (wind <= 102)
            {
                re.WindLevel = "9";
                re.WindColor = Colors.MediumBlue;
            }
            else if (wind <= 117)
            {
                re.WindLevel = "10";
                re.WindColor = Colors.DarkBlue;
            }
            else if (wind <= 134)
            {
                re.WindLevel = "11";
                re.WindColor = Colors.MidnightBlue;
            }
            else if (wind <= 149)
            {
                re.WindLevel = "12";
                re.WindColor = Colors.Navy;
            }
            else if (wind <= 166)
            {
                re.WindLevel = "13";
                re.WindColor = Colors.Navy;
            }
            else if (wind <= 183)
            {
                re.WindLevel = "14";
                re.WindColor = Colors.Navy;
            }
            else if (wind <= 201)
            {
                re.WindLevel = "15";
                re.WindColor = Colors.Navy;
            }
            else if (wind <= 220)
            {
                re.WindLevel = "16";
                re.WindColor = Colors.Navy;
            }
            else
            {
                re.WindLevel = "17";
                re.WindColor = Colors.Navy;
            }

            return re;
        }
    }
}
