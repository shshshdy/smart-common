
using System.Collections.Generic;
using Smart.Shared.Interfaces;

namespace Smart.Shared.Systems.Dto
{
    /// <summary>
    /// 字典输出
    /// </summary>
    public class SysDicListOutputDto : IListOutputDto<SysDicOutputDto>
    {
        /// <summary>
        /// 总页数
        /// </summary>
        public long TotalPageSize { get; set; }
        /// <summary>
        /// 数据列表
        /// </summary>
        public IEnumerable<SysDicOutputDto> List { get; set; }
    }
}

