using Smart.Infrastructure.Authenticate;
using Smart.Infrastructure.Freesql.Entities;
using Smart.Infrastructure.Freesql.Responsitories;
using FreeSql;

namespace Smart.Infrastructure.Freesql.Responsitories
{
    public class SysDicResponsitory : RepositoryBase<SysDic>, ISysDicResponsitory
    {
        public SysDicResponsitory(IFreeSql orm, UnitOfWorkManager uow, IUser user) : base(orm, uow, user)
        {
        }
    }
}
