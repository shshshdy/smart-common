using Smart.Infrastructure.Freesql.Entities;

namespace Smart.Domain.Entities
{
    /// <summary>
    /// 权限
    /// </summary>
    public class Permission : EntityBase<long>, IEntitySoftDelete
    {

        /// <summary>
        /// 权限路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 权限说明
        /// </summary>
        public string Description { get; set; }
    }
}
