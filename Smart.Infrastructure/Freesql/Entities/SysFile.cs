using Smart.Infrastructure.Freesql.Entities;

namespace Smart.Infrastructure.Freesql.Entities
{
    public class SysFile : EntityBase<long>, IEntitySoftDelete
    {
        /// <summary>
        /// 文件身份
        /// </summary>
        public string Md5 { get; set; }
         /// <summary>
         /// 上传时间
         /// </summary>
        public string FileDate { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 是否上传
        /// </summary>
        public bool Uploaded { get; set; }

        /// <summary>
        /// 是否使用过
        /// </summary>
        public bool Used { get; set; }
    }
}
