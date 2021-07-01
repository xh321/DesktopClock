using System;

namespace DesktopClock.Utils
{
    public static class StringUtils
    {
        public static string GetBetween(string text, string left, string right)
        {
            //判断是否为null或者是empty
            if (string.IsNullOrEmpty(left) || string.IsNullOrEmpty(right) || string.IsNullOrEmpty(text))
                return string.Empty;

            var leftIndex = text.IndexOf(left, StringComparison.Ordinal); //搜索left的位置

            //判断是否找到left
            if (leftIndex == -1)
                return string.Empty;

            leftIndex += left.Length; //取出left右边文本起始位置

            var rightIndex = text.IndexOf(right, leftIndex, StringComparison.Ordinal); //从left的右边开始寻找right

            //判断是否找到right
            if (rightIndex == -1)
                return "";

            return text[leftIndex..rightIndex]; //返回查找到的文本
        }

        public static string String_GetLeft(string in_str, string find_str)
        {
            var re_1 = "";
            var index = in_str.IndexOf(find_str, StringComparison.Ordinal);
            if (index > 0)
            {
                re_1 = in_str[..index];
            }

            return re_1;
        }

        public static string String_GetRight_Last(string in_str, string find_str)
        {
            var sz = in_str.Split(find_str.ToCharArray());
            var re = "";
            if (true)
            {
                if (sz.Length > 0)
                {
                    re = sz[^1];
                }
            }

            return re;
        }
    }
}