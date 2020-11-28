using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Smart.Domain.Entities.Enums
{
    public class EnumHelper
    {
        /// <summary>
        /// 程序集枚举转为Json
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <param name="normal">包含本程序集</param>
        /// <returns></returns>
        public static JObject GetEntitiyEnums(Assembly assembly, bool normal = true)
        {
            var json = new JObject();

            if (normal)
            {
                Enumber2Json(typeof(EnumHelper).Assembly, json);
            }

            if (assembly != null)
            {
                Enumber2Json(assembly, json);

            }
            return json;

            static void Enumber2Json(Assembly assembly, JObject json)
            {
                var enums = assembly.GetTypes().Where(p => p.IsEnum).ToList();
                foreach (var item in enums)
                {
                    var jobj = new JArray();
                    foreach (var value in Enum.GetValues(item))
                    {
                        var k = value.ToString();
                        var v = Convert.ToInt32(value);
                        var field = item.GetField(k);
                        object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);    //获取描述属性
                        jobj.Add(new JObject { { "key", k }, { "value", v }, { "description", objs.Length == 0 ? "" : ((DescriptionAttribute)objs[0]).Description } });
                    }
                    json[item.Name.Substring(0, 1).ToLower() + item.Name.Substring(1)] = jobj;
                }
            }
        }

    }
}
