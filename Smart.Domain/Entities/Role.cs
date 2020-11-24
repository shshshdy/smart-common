using Smart.Infrastructure.Freesql.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Smart.Domain.Entities
{
    /// <summary>
    /// 角色
    /// </summary>
    public class Role : EntityBase<long>, IEntitySoftDelete
    {
        /// <summary>
        /// 角色名
        /// </summary>
        public string Name { get; set; }
    }
}
