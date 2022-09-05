using System;

namespace DesktopClock.Extensions
{
    public static class StringExtension
    {
        #region 拓展方法

        /// <summary>
        /// 获取源字符串的两个子字符串中间的内容
        /// </summary>
        /// <param name="text">源字符串</param>
        /// <param name="left">左侧子字符串</param>
        /// <param name="right">右侧子字符串</param>
        /// <returns>中间的内容</returns>
        public static string GetBetween(this string text, string left, string right)
        {
            // 判空
            if (text is null) throw new ArgumentNullException(nameof(text));
            if (string.IsNullOrEmpty(left) || string.IsNullOrEmpty(right)) return text;

            // 查找左侧索引
            var leftIndex = text.IndexOf(left, StringComparison.Ordinal);
            if (leftIndex == -1)
                return string.Empty;
            // 查找右侧索引
            var rightIndex = text.IndexOf(right, leftIndex, StringComparison.Ordinal);
            if (rightIndex == -1)
                return string.Empty;

            //返回查找到的文本
            return text[(leftIndex + left.Length)..rightIndex];
        }

        /// <summary>
        /// 获取源字符串的子字符串左侧的内容
        /// </summary>
        /// <param name="text">源字符串</param>
        /// <param name="findStr">子字符串</param>
        /// <returns>左侧的内容</returns>
        public static string GetLeft(this string text, string findStr)
        {
            // 判空
            if (text is null) throw new ArgumentNullException(nameof(text));
            if (string.IsNullOrEmpty(findStr)) return text;

            // 查找索引
            var index = text.IndexOf(findStr);

            //返回查找到的文本
            return index == -1
                ? string.Empty
                : text[..(index + 1)].ToString();
        }

        /// <summary>
        /// 获取源字符串的最后一个子字符串右侧的内容
        /// </summary>
        /// <param name="text">源字符串</param>
        /// <param name="findStr">子字符串</param>
        /// <returns>右侧的内容</returns>
        public static string GetRightLast(this string text, string findStr)
        {
            // 判空
            if (text is null) throw new ArgumentNullException(nameof(text));
            if (string.IsNullOrEmpty(findStr)) return text;

            // 查找索引
            var lastIndex = text.LastIndexOf(findStr);

            //返回查找到的文本
            return lastIndex == -1
                ? string.Empty
                : text[(lastIndex + findStr.Length)..].ToString();
        }

        #endregion
    }
}
