using System;
namespace Smart.Infrastructure.Freesql.Attributes
{
    public class NavigateDicAttribute : Attribute
    {
        public string DicCode { get; private set; }
        public string FieldName { get; private set; }
        /// <summary>
        /// 系统字典导航
        /// </summary>
        /// <param name="code">字典类型,请考虑全局常量定义引用</param>
        /// <param name="fieldName">对应字段</param>
        public NavigateDicAttribute(string code,string fieldName)
        {
            DicCode = code;
            FieldName = fieldName;
        }
    }
}
