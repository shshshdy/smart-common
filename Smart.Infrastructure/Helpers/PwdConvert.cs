using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Smart.Infrastructure.Helpers
{
    /// <summary>
    /// 密码
    /// </summary>
    public static class PwdConvert
    { 
        private static readonly byte[] Key64 = { 42, 16, 93, 56, 78, 8, 218, 223 };
        private static readonly byte[] Iv64 = { 55, 103, 46, 79, 36, 89, 167, 7 };

        #region 加密
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="pwd">明文密码</param>
        /// <returns></returns>
        public static string PwdEncode(this string pwd)
        {
            pwd = Encrypt(pwd);
            var numReg = new Regex("[0-9]");
            var mReg = new Regex("[a-z]");
            var lReg = new Regex("[A-Z]");
            var values = pwd.ToCharArray();
            var newStr = string.Empty;
            foreach (var value in values)
            {
                var v = value.ToString(CultureInfo.InvariantCulture);
                if (numReg.IsMatch(v))
                {
                    newStr += Convert.ToInt32(value) + 5;
                }
                else if (mReg.IsMatch(v))
                {
                    var s = Convert.ToInt32(value) - 96;

                    newStr += "00".Substring(s.ToString(CultureInfo.InvariantCulture).Length) + s;
                }
                else if (lReg.IsMatch(v))
                {
                    newStr += Convert.ToInt32(value) - 38;
                }
                else
                {
                    newStr += "0" + value;
                }
            }
            return newStr;
        }
        //标准的DES加密
        private static string Encrypt(string pwd)
        {
            if (pwd != "")
            {
                var cryptoProvider = new DESCryptoServiceProvider();
                var ms = new MemoryStream();
                var cs = new CryptoStream(ms, cryptoProvider.CreateEncryptor(Key64, Iv64), CryptoStreamMode.Write);
                var sw = new StreamWriter(cs);
                sw.Write(pwd);
                sw.Flush();
                cs.FlushFinalBlock();
                ms.Flush();
                return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
            }
            return "";
        }
        #endregion

        #region 解密
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="pwd">密文密码</param>
        /// <returns></returns>
        public static string PwdDecode(this string pwd)
        {
            var newStr = string.Empty;
            var reg = new Regex("^[0-9]{1,}$");
            for (var i = 0; i < pwd.Length / 2; i++)
            {
                var s = pwd.Substring(i * 2, 2);
                if (!reg.IsMatch(s))
                {
                    newStr += s.Substring(1);
                }
                else
                {
                    var v = Convert.ToInt32(s);
                    if (v < 27)
                    {
                        newStr += Convert.ToChar(v + 96);
                    }
                    else if (v < 53)
                    {
                        newStr += Convert.ToChar(v + 38);
                    }
                    else
                    {
                        newStr += Convert.ToChar(v - 5);
                    }
                }

            }
            return Decrypt(newStr);
        }
       
        //标准的DES解密
        private static string Decrypt(string pwd)
        {
            if (pwd != "")
            {
                var cryptoProvider = new DESCryptoServiceProvider();
                byte[] buffer = Convert.FromBase64String(pwd);
                var ms = new MemoryStream(buffer);
                var cs = new CryptoStream(ms, cryptoProvider.CreateDecryptor(Key64, Iv64), CryptoStreamMode.Read);
                var sr = new StreamReader(cs);
                return sr.ReadToEnd();
            }
            return "";
        }
        #endregion
    }
}
