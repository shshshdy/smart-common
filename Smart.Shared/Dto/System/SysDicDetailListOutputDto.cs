﻿
using System.Collections.Generic;
using Smart.Shared.Interfaces;

namespace Smart.Shared.Systems.Dto
{
    /// <summary>
    /// 字典详情输出dto
    /// </summary>
    public class SysDicDetailListOutputDto : IListOutputDto<SysDicDetailOutputDto>
    {
        /// <summary>
        /// 总页数
        /// </summary>
        public long TotalPageSize { get; set; }
        /// <summary>
        /// 数据列表
        /// </summary>
        public IEnumerable<SysDicDetailOutputDto> List { get; set; }
    }
}

