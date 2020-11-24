using Smart.Infrastructure.Helpers;
using Microsoft.International.Converters.PinYinConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Smart.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// 判断字符串是否为Null、空
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsNull(this string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }

        /// <summary>
        /// 判断字符串是否不为Null、空
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool NotNull(this string s)
        {
            return !string.IsNullOrWhiteSpace(s);
        }

        /// <summary>
        /// 与字符串进行比较，忽略大小写
        /// </summary>
        /// <param name="s"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool EqualsIgnoreCase(this string s, string value)
        {
            return s.Equals(value, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 首字母转小写
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string FirstCharToLower(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;

            string str = s.First().ToString().ToLower() + s.Substring(1);
            return str;
        }

        /// <summary>
        /// 首字母转大写
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string FirstCharToUpper(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;

            string str = s.First().ToString().ToUpper() + s.Substring(1);
            return str;
        }

        /// <summary>
        /// 转为Base64，UTF-8格式
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToBase64(this string s)
        {
            return s.ToBase64(Encoding.UTF8);
        }

        /// <summary>
        /// 转为Base64
        /// </summary>
        /// <param name="s"></param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string ToBase64(this string s, Encoding encoding)
        {
            if (s.IsNull())
                return string.Empty;

            var bytes = encoding.GetBytes(s);
            return bytes.ToBase64();
        }
        /// <summary>
        /// 转为汉语拼音
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToPinyin(this string s)
        {
            return GetPinyin(s, false);
        }
        /// <summary>
        /// 转为汉语拼音首字母
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToPinyinForShort(this string s)
        {
            return GetPinyin(s, true);
        }


        #region 私有方法
        /// <summary>
        /// 汉字转拼音，多音字返回多组组合
        /// </summary>
        /// <param name="s"></param>
        /// <param name="isFirst">是否只返回首字母</param>
        /// <returns></returns>
        private static string GetPinyin(string s, bool isFirst)
        {
            var chs = s.ToCharArray();
            //记录每个汉字的全拼
            Dictionary<int, List<string>> totalPingYins = new Dictionary<int, List<string>>();
            for (int i = 0; i < chs.Length; i++)
            {
                var pinyins = new List<string>();
                var ch = chs[i];
                //是否是有效的汉字
                if (ChineseChar.IsValidChar(ch))
                {
                    ChineseChar cc = new ChineseChar(ch);
                    pinyins = cc.Pinyins.Where(p => !string.IsNullOrWhiteSpace(p)).ToList();
                }
                else
                {
                    pinyins.Add(ch.ToString());
                }

                //去除声调
                pinyins = pinyins.ConvertAll(p => Regex.Replace(p, @"\d", "").ToLower());
                //去重
                pinyins = pinyins.Where(p => !string.IsNullOrWhiteSpace(p)).Distinct().ToList();
                if (pinyins.Any())
                {
                    totalPingYins[i] = pinyins;
                }
            }
            var pinYins = new List<string>();
            var firstPinYins = new List<string>();
            foreach (var pinyins in totalPingYins)
            {
                var items = pinyins.Value;

                if (pinYins.Count <= 0)
                {
                    pinYins = items;
                    firstPinYins = items.ConvertAll(p => p.Substring(0, 1)).Distinct().ToList();
                }
                else
                {
                    if (isFirst)
                    {
                        //首字母循环匹配
                        var newFirstPinYins = new List<string>();
                        foreach (var pinyin in firstPinYins)
                        {
                            newFirstPinYins.AddRange(items.Select(item => pinyin + item.Substring(0, 1)));
                        }
                        newFirstPinYins = newFirstPinYins.Distinct().ToList();
                        firstPinYins = newFirstPinYins;

                    }
                    else
                    {
                        //全拼循环匹配
                        var newPinYins = new List<string>();
                        foreach (var pinyin in pinYins)
                        {
                            newPinYins.AddRange(items.Select(item => pinyin + item));
                        }
                        newPinYins = newPinYins.Distinct().ToList();
                        pinYins = newPinYins;
                    }
                }
            }

            return string.Join('|', isFirst ? firstPinYins : pinYins);
        }
        #endregion
    }
}
