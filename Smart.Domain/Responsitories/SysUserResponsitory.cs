using Smart.Domain.Entities;
using Smart.Domain.Responsitories;
using Smart.Infrastructure.Authenticate;
using Smart.Infrastructure.Freesql.Responsitories;
using FreeSql;
using System;

namespace Smart.Repositories
{
    public class SysUserResponsitory : RepositoryBase<SysUser>, ISysUserResponsitory
    {
        public SysUserResponsitory(IFreeSql orm, UnitOfWorkManager uow, IUser user) : base(orm, uow, user)
        {
        }
    }
}
