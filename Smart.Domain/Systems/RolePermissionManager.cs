//=============================================================
// 创建人:            ssd
// 创建时间:          2020/9/20 16:27:21
// 邮箱：             1292934053@qq.com
//==============================================================
using Smart.Domain.Entities;
using Smart.Domain.Responsitories;
using Smart.Domain.Systems.Interfaces;
using System.Collections.Generic;

namespace Smart.Domain.Systems
{
    public class RolePermissionManager : IRolePermissionManager
    {
        IRolePermissionResponsitory _responsitory;
        public RolePermissionManager(IRolePermissionResponsitory responsitory)
        {
            _responsitory = responsitory;
        }

        public IEnumerable<RolePermission> GetAll()
        {
            var result = _responsitory.Select.ToList();
            return result;
        }

        public IEnumerable<RolePermission> GetAll(int pageIndex, int pageSize)
        {
            var result = _responsitory.Where(p => true).OrderBy(p => p.Id).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToList();
            return result;
        }

        public RolePermission Get(long id)
        {
            var user = _responsitory.Where(p => p.Id == id).First();
            return user;
        }

        public long Count()
        {
            var count = _responsitory.Select.Count();
            return count;
        }

      
        public RolePermission CreatOrUpdate(RolePermission rolePermission)
        {
            var result = _responsitory.InsertOrUpdate(rolePermission);
            return result;
        }
    }
}