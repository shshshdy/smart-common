
using Smart.Domain.Entities;
using Smart.Domain.Responsitories;
using Smart.Infrastructure.Authenticate;
using Smart.Infrastructure.Freesql.Responsitories;
using FreeSql;

namespace Smart.Repositories
{
    public class PermissionResponsitory : RepositoryBase<Permission>, IPermissionResponsitory
    {
        public PermissionResponsitory(IFreeSql orm, UnitOfWorkManager uow, IUser user) : base(orm, uow, user)
        {

        }
    }
}
