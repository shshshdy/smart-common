
using Smart.Domain.Entities;
using Smart.Domain.Responsitories;
using Smart.Infrastructure.Authenticate;
using Smart.Infrastructure.Freesql.Responsitories;
using FreeSql;

namespace Smart.Repositories
{
    public class RolePermissionResponsitory : RepositoryBase<RolePermission>, IRolePermissionResponsitory
    {
        public RolePermissionResponsitory(IFreeSql orm, UnitOfWorkManager uow, IUser user) : base(orm, uow, user)
        {

        }
    }
}
