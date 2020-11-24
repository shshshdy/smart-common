using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart.Application.Files.Models
{
    /// <summary>
    /// 文件分片排序
    /// </summary>
    public class FileSort
    {
        /// <summary>
        /// 文件分片编号
        /// </summary>
        public const string PART_NUMBER = ".partNumber-";
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件分片号
        /// </summary>
        public int PartNumber { get; set; }
    }
}
