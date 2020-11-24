
using Smart.Domain.Entities;
using Smart.Domain.Responsitories;
using Smart.Infrastructure.Authenticate;
using Smart.Infrastructure.Freesql.Responsitories;
using FreeSql;

namespace Smart.Repositories
{
    public class UserRoleResponsitory : RepositoryBase<UserRole>, IUserRoleResponsitory
    {
        public UserRoleResponsitory(IFreeSql orm, UnitOfWorkManager uow, IUser user) : base(orm, uow, user)
        {

        }
    }
}
