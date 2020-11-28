using System.Text.RegularExpressions;

namespace Freesql.Tool.Helpers
{
    public static class TextHelper
    {
        /// <summary>
        /// 首字母大写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToFirstUp(this string str)
        {
            return str.Substring(0, 1).ToUpper() + str.Substring(1);
        }
        /// <summary>
        /// 首字母小写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToFirstLow(this string str)
        {
            return str.Substring(0, 1).ToLower() + str.Substring(1);
        }
        /// <summary>
        /// 转换为复数形式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToPlural(this string str)
        {
            Regex plural1 = new Regex("(?<keep>[^aeiouAEIOU])y$");
            Regex plural2 = new Regex("(?<keep>[aeiouAEIOU]y)$");
            Regex plural3 = new Regex("(?<keep>[sxzhSXZH])$");
            Regex plural4 = new Regex("(?<keep>[^sxzhySXZHY])$");

            if (plural1.IsMatch(str))
                return plural1.Replace(str, "${keep}ies");
            else if (plural2.IsMatch(str))
                return plural2.Replace(str, "${keep}s");
            else if (plural3.IsMatch(str))
                return plural3.Replace(str, "${keep}es");
            else if (plural4.IsMatch(str))
                return plural4.Replace(str, "${keep}s");

            return str;
        }
        // <summary>
        /// 转为单数形式
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ToSingle(this string name)
        {
            Regex plural1 = new Regex("(?<keep>[^aeiou])ies$");
            Regex plural2 = new Regex("(?<keep>[aeiou]y)s$");
            Regex plural3 = new Regex("(?<keep>[sxzh])es$");
            Regex plural4 = new Regex("(?<keep>[^sxzhyu])s$");

            if (plural1.IsMatch(name))
                return plural1.Replace(name, "${keep}y");
            else if (plural2.IsMatch(name))
                return plural2.Replace(name, "${keep}");
            else if (plural3.IsMatch(name))
                return plural3.Replace(name, "${keep}");
            else if (plural4.IsMatch(name))
                return plural4.Replace(name, "${keep}");

            return name;
        }
        /// <summary>
        /// 转换为"首3位.最后单词首3位"
        /// </summary>
        /// <param name="split"></param>
        /// <returns></returns>
        public static string ToJsName(this string name)
        {
            return string.Join('.', FirstLastStr(name));
            
        }
        
        /// <summary>
        /// 转换为 “首3位/最后单词道3位/index”
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ToVuePath(this string name)
        {
            return string.Join('/', FirstLastStr(name));
        }

        private static string[] FirstLastStr(string name)
        {
            var lastWord = "";
            for (int j = name.Length - 1; j >= 0; j--)
            {
                var temp = name[j].ToString();
                if (Regex.IsMatch(temp, "[A-Z]"))
                {
                    lastWord = name.Substring(j,3).ToLower();
                    break;
                }
            }
            var firstWord = name.Substring(0, 3).ToLower();
            return firstWord == lastWord || string.IsNullOrEmpty(lastWord) ? new string[] { name } : new string[] { firstWord, lastWord };
        }
    }
}
