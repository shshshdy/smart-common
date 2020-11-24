//=============================================================
// 创建人:            ssd
// 创建时间:          2020/9/20 16:27:19
// 邮箱：             1292934053@qq.com
//==============================================================
using Smart.Domain.Entities;
using Smart.Domain.Responsitories;
using Smart.Domain.Systems.Interfaces;
using System.Collections.Generic;

namespace Smart.Domain.Systems
{
    public class RoleManager : IRoleManager
    {
        IRoleResponsitory _responsitory;
        public RoleManager(IRoleResponsitory responsitory)
        {
            _responsitory = responsitory;
        }

        public IEnumerable<Role> GetAll()
        {
            var result = _responsitory.Select.ToList();
            return result;
        }

        public IEnumerable<Role> GetAll(int pageIndex, int pageSize)
        {
            var result = _responsitory.Where(p => true).OrderBy(p => p.Id).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToList();
            return result;
        }

        public Role Get(long id)
        {
            var user = _responsitory.Where(p => p.Id == id).First();
            return user;
        }

        public long Count()
        {
            var count = _responsitory.Select.Count();
            return count;
        }

      
        public Role CreatOrUpdate(Role role)
        {
            var result = _responsitory.InsertOrUpdate(role);
            return result;
        }
    }
}