using Smart.Infrastructure.Authenticate;
using Smart.Infrastructure.Freesql.Entities;
using FreeSql;

namespace Smart.Infrastructure.Freesql.Responsitories
{
    public class SysFileResponsitory : RepositoryBase<SysFile>, ISysFileResponsitory
    {
        public SysFileResponsitory(IFreeSql orm, UnitOfWorkManager uow, IUser user) : base(orm, uow, user)
        {

        }
    }
}
