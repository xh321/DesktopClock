using System;

namespace DesktopClock.Utils
{
    public static class StringUtils
    {
        public static string GetMiddle(this string text, string left, string right)
        {
            if (string.IsNullOrEmpty(text)) return text;

            var leftIndex = text.IndexOf(left, StringComparison.Ordinal);

            throw new NotImplementedException();
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

            int Lindex = text.IndexOf(left, StringComparison.Ordinal); //搜索left的位置

            if (Lindex == -1)
            {
                //判断是否找到left
                return "";
            }

            Lindex = Lindex + left.Length; //取出left右边文本起始位置

            int Rindex = text.IndexOf(right, Lindex, StringComparison.Ordinal); //从left的右边开始寻找right

            if (Rindex == -1)
            {
                //判断是否找到right
                return "";
            }

            return text.Substring(Lindex, Rindex - Lindex); //返回查找到的文本
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

        #region 拓展方法

        /// <summary>
        /// 获取字符串中某个子字符串右侧的字符串
        /// </summary>
        /// <param name="text">源字符串</param>
        /// <param name="findStr">查找的字符串</param>
        /// <returns>查找字符串右侧的字符串</returns>
        public static string GetRight(this string text, string findStr)
        {
            if (text is null) throw new ArgumentNullException(nameof(text));
            if (string.IsNullOrEmpty(findStr)) return text;

            var index = text.IndexOf(findStr);

            return index == -1
                ? string.Empty
                : text[(index + findStr.Length)..].ToString();
        }

        public static string GetRightLast(this string text, string findStr)
        {
            if (text is null) throw new ArgumentNullException(nameof(text));
            if (string.IsNullOrEmpty(findStr)) return text;

            var lastIndex = text.LastIndexOf(findStr);

            return lastIndex == -1
                ? string.Empty
                : text[(lastIndex + findStr.Length)..].ToString();
        }

        public static string GetLeft(this string text, string findStr)
        {
            if (text is null) throw new ArgumentNullException(nameof(text));
            if (string.IsNullOrEmpty(findStr)) return text;

            var index = text.IndexOf(findStr);

            return index == -1
                ? string.Empty
                : text[..(index + 1)].ToString();
        }

        #endregion
    }
}
