using Smart.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Smart.Application.Dtos
{
    /// <summary>
    /// 输入dto
    /// </summary>
    public class InputDto: IInputDto
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }
    }
}
