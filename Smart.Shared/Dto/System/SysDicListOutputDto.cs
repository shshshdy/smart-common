
using System.Collections.Generic;
using Smart.Application.Interfaces;

namespace Smart.Application.Systems.Dto
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

