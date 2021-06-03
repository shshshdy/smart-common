using Smart.Shared.Dtos;
using System;

namespace Smart.Shared.Files.Dto
{
    ///<summary>
    ///输出Dto
    ///</summary>
    public class SysFileOutputDto : BaseOutputDto
    {
        /// <summary>
        /// MD5
        /// </summary>
        public string Md5 { get; set; }
        /// <summary>
        /// 文件日期
        /// </summary>
        public string FileDate { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 是否上传成功
        /// </summary>
        public bool Uploaded { get; set; }
    }
}
