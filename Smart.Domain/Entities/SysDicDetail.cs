using System;
using Smart.Infrastructure.Freesql.Entities;
using FreeSql.DataAnnotations;

namespace Smart.Domain.Entities
{
    /// <summary>
    /// 字典
    /// </summary>
    public class SysDicDetail : EntityBase<long>
    {
        /// <summary>
        /// 字典类型
        /// </summary>
        public long DicId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        [Column(CanUpdate = false)]
        public string Value { get; set; }
        /// <summary>
        ///  排序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool Enabled { get; set; } = false;

        /// <summary>
        /// 字典类
        /// </summary>
        [Navigate(nameof(DicId))]
        public virtual SysDic SysDic { get; set; }


    }
}
