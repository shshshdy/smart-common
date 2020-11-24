using FreeSql.DataAnnotations;

namespace Smart.Infrastructure.Freesql.Entities
{
    /// <summary>
    /// 字典
    /// </summary>
    public class SysDic : EntityBase<long>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 编码,请使用Constens类下的常量
        /// </summary>
        [Column(CanUpdate = false)]
        public string Code { get; set; }
    }
}
