using System.Collections.Generic;
using Smart.Shared.Interfaces;

namespace Smart.Shared.Files.Dto
{
    /// <summary>
    /// 文件输出
    /// </summary>
    public class SysFileListOutputDto : IListOutputDto<SysFileOutputDto>
    {
        /// <summary>
        /// 总页数
        /// </summary>
        public long TotalPageSize { get; set; }
        /// <summary>
        /// 数据集
        /// </summary>
        public IEnumerable<SysFileOutputDto> List { get; set; }
    }
}
