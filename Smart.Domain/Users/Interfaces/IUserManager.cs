using Smart.Domain.Entities;
using System.Collections.Generic;

namespace Smart.Domain.Users.Interfaces
{
    public interface IUserManager: IManager<SysUser>
    {
        SysUser CheckUser(string userName);

        SysUser CheckUser(string userName, string password);

        SysUser Get(string userName);

        IEnumerable<SysUser> GetList(string key);

        bool CheckPermission(long userId, string path,bool checkFather);
        IEnumerable<Permission> GetPermissions(long userId);
    }
}
