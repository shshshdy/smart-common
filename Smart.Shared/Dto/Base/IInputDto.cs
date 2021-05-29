using System;
using System.Collections.Generic;
using System.Text;

namespace Smart.Application.Interfaces
{
    /// <summary>
    /// 传参接口
    /// </summary>
    public interface IInputDto
    {
        /// <summary>
        /// 当前页索引
        /// </summary>
        int PageIndex { get; set; }
        /// <summary>
        /// 分页大小
        /// </summary>
        int PageSize { get; set; }
    }
}
