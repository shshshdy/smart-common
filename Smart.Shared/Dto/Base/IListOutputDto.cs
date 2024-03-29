﻿using System.Collections.Generic;

namespace Smart.Shared.Interfaces
{
    /// <summary>
    /// 返回数据集DTO
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IListOutputDto<T>
    {
        /// <summary>
        /// 总页数
        /// </summary>
        long TotalPageSize { get; set; }
        /// <summary>
        /// 列表
        /// </summary>

        IEnumerable<T> List { get; set; }
    }
}
