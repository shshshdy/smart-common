//=============================================================
// 创建人:            ssd
// 创建时间:          2020/9/20 16:31:41
// 邮箱：             1292934053@qq.com
//==============================================================
using Smart.Domain.Entities;
using Smart.Domain.Responsitories;
using Smart.Domain.Systems.Interfaces;
using System.Collections.Generic;

namespace Smart.Domain.Systems
{
    public class UserRoleManager : IUserRoleManager
    {
        IUserRoleResponsitory _responsitory;
        public UserRoleManager(IUserRoleResponsitory responsitory)
        {
            _responsitory = responsitory;
        }

        public IEnumerable<UserRole> GetAll()
        {
            var result = _responsitory.Select.ToList();
            return result;
        }

        public IEnumerable<UserRole> GetAll(int pageIndex, int pageSize)
        {
            var result = _responsitory.Where(p => true).OrderBy(p => p.Id).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToList();
            return result;
        }

        public UserRole Get(long id)
        {
            var user = _responsitory.Where(p => p.Id == id).First();
            return user;
        }

        public long Count()
        {
            var count = _responsitory.Select.Count();
            return count;
        }

      
        public UserRole CreatOrUpdate(UserRole userRole)
        {
            var result = _responsitory.InsertOrUpdate(userRole);
            return result;
        }
    }
}