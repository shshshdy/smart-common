using Smart.Infrastructure.Freesql.Entities;
using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Smart.Domain.Entities
{
    public class RolePermission : EntityBase<long>, IEntitySoftDelete
    {
        public long RoleId { get; set; }
        public long PermissionId { get; set; }

        [Navigate(nameof(RoleId))]
        public Role Role { get; set; }

        [Navigate(nameof(PermissionId))]
        public Permission Permission { get; set; }
    }
}
