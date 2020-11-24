using Smart.Infrastructure.Freesql.Entities;
using Smart.Domain.Entities.Enums;
using System;

namespace Smart.Domain.Entities
{
    public class SysCode : EntityBase<long>, IEntitySoftDelete
    {
        /// <summary>
        /// 前缀
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 当前日期
        /// </summary>
        public DateTime CurrentDate { get; set; }
    }
}
