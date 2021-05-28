using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart.Shared.Dto
{
    public class FileInput
    {
        /// <summary>
        /// 文件流
        /// </summary>
        public byte[] Stream { get; set; }
        /// <summary>
        /// MD5
        /// </summary>
        public string Md5 { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public string FileType { get; set; }
        /// <summary>
        /// 上传结束
        /// </summary>
        public bool IsComplate { get; set; }
        /// <summary>
        /// 当前切片
        /// </summary>
        public int ChunkIndex { get; set; }
        /// <summary>
        /// 唯一标识，防止不同用户上传同一文件
        /// </summary>
        public string Guid { get; set; }
    }
}
