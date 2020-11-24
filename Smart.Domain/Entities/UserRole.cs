using Smart.Infrastructure.Freesql.Entities;
using FreeSql.DataAnnotations;

namespace Smart.Domain.Entities
{
    public class UserRole : EntityBase<long>, IEntitySoftDelete
    {
        public long UserId { get; set; }

        public long RoleId { get; set; }

        [Navigate(nameof(RoleId))]
        public Role Role { get; set; }
    }
}
