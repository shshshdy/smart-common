using System;
using System.Collections.Generic;
using System.Text;

namespace Smart.Infrastructure.Freesql
{
    /// <summary>
    /// 标记为存放文件信息的md5字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FileAttribute : Attribute
    {

    }
}
