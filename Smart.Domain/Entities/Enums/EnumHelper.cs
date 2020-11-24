using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Smart.Domain.Entities.Enums
{
    public class EnumHelper
    {
        public static JObject GetEntitiyEnums(Assembly assembly = null)
        {
            var json = new JObject();
            if (assembly == null)
            {
                assembly = typeof(EnumHelper).Assembly;
            }
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
            return json;

        }

    }
}
