
using Smart.Domain.Entities;
using Smart.Domain.Responsitories;
using Smart.Infrastructure.Authenticate;
using Smart.Infrastructure.Freesql.Responsitories;
using FreeSql;

namespace Smart.Repositories
{
    public class RoleResponsitory : RepositoryBase<Role>, IRoleResponsitory
    {
        public RoleResponsitory(IFreeSql orm, UnitOfWorkManager uow, IUser user) : base(orm, uow, user)
        {

        }
    }
}
