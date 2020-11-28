using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.XPath;
using Freesql.Tool.Models;
using FreeSql.DataAnnotations;

namespace Freesql.Tool.Helpers
{
    public static class ReflectionHelper
    {
        public static bool IsNavgation(this PropertyInfo info)
        {
            var attr = info.GetCustomAttribute<NavigateAttribute>(true);
            return attr != null;
        }
        /// <summary>
        /// 获取属性列表
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="classPath"></param>
        /// <param name="isEnum"></param>
        /// <returns></returns>
        public static List<Property> GetProperties(string assembly,string classPath,out bool isEnum)
        {
            isEnum = false;
            var list = new List<Property>();
            string[] cloums = new string[] { "Id", "Version", "IsDeleted", "CreatedUserId", "CreatedUserName", "CreatedTime", "ModifiedUserId", "ModifiedUserName", "ModifiedTime" };
            var name = Path.GetFileName(assembly).Replace(".dll", "",StringComparison.OrdinalIgnoreCase) + ".xml";
            var xmlPath = assembly.Replace(Path.GetFileName(assembly), name);
            var doc = new XPathDocument(xmlPath);
            var nav = doc.CreateNavigator();
            Assembly ass = Assembly.LoadFrom(assembly);
            var type = ass.GetType(classPath);
            if (type != null)
            {
                foreach (var item in type.GetProperties())
                {
                    if (cloums.Any(p => p == item.Name))
                    {
                        continue;
                    }
                    if (item.IsNavgation())
                    {
                        continue;
                    }
                    var property = new Property { Name = item.Name};
                    

                    if (item.PropertyType == typeof(bool))
                    {
                        property.Type = "bool";
                    }
                    else if (item.PropertyType == typeof(long))
                    {
                        property.Type = "long";
                    }
                    else if (item.PropertyType == typeof(Nullable<long>))
                    {
                        property.Type = "long?";
                    }
                    else if (item.PropertyType == typeof(Nullable<bool>))
                    {
                        property.Type = "bool?";
                    }
                    else if (item.PropertyType == typeof(int))
                    {
                        property.Type = "int";
                    }
                    else if (item.PropertyType == typeof(Nullable<int>))
                    {
                        property.Type = "int?";
                    }
                    else if (item.PropertyType.IsEnum)
                    {
                        isEnum = true;
                        property.Type = item.Name;
                    }
                    else if (item.PropertyType == typeof(string))
                    {
                        property.Type = "string";
                    }
                    else if (item.PropertyType == typeof(decimal))
                    {
                        property.Type = "decimal";
                    }
                    else if (item.PropertyType == typeof(Nullable<decimal>))
                    {
                        property.Type = "decimal?";
                    }
                    else if (item.PropertyType == typeof(DateTime))
                    {
                        property.Type = "DateTime";
                    }
                    else if (item.PropertyType == typeof(Nullable<DateTime>))
                    {
                        property.Type = "DateTime?";
                    }
                    else
                    {
                        property.Type = "string";
                    }
                    var xpath = $"/doc/members/member[@name='P:{classPath}.{item.Name}']/summary";
                    var description = nav.SelectSingleNode(xpath)?.Value?.Trim();
                    property.Description = description;
                    list.Add(property);
                }
            }
            return list;
        }

        public static string GetDescription(string assembly, string classPath)
        {
            var name = Path.GetFileName(assembly).Replace(".dll", "", StringComparison.OrdinalIgnoreCase) + ".xml";
            var xmlPath = assembly.Replace(Path.GetFileName(assembly), name);
            var doc = new XPathDocument(xmlPath);
            var nav = doc.CreateNavigator();
            var xpath = $"/doc/members/member[@name='T:{classPath}']/summary";
            var description = nav.SelectSingleNode(xpath)?.Value?.Trim();
            return description;
        }
    }
}
