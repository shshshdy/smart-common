//=============================================================
// 创建人:            ssd
// 创建时间:          2020/9/19 22:44:21
// 邮箱：             1292934053@qq.com
//==============================================================
using Smart.Domain.Entities;
using Smart.Domain.Responsitories;
using Smart.Domain.Systems.Interfaces;
using System.Collections.Generic;

namespace Smart.Domain.Systems
{
    public class PermissionManager : IPermissionManager
    {
        IPermissionResponsitory _responsitory;
        public PermissionManager(IPermissionResponsitory responsitory)
        {
            _responsitory = responsitory;
        }

        public IEnumerable<Permission> GetAll()
        {
            var result = _responsitory.Select.ToList();
            return result;
        }

        public IEnumerable<Permission> GetAll(int pageIndex, int pageSize)
        {
            var result = _responsitory.Where(p => true).OrderBy(p => p.Id).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToList();
            return result;
        }

        public Permission Get(long id)
        {
            var user = _responsitory.Where(p => p.Id == id).First();
            return user;
        }

        public long Count()
        {
            var count = _responsitory.Select.Count();
            return count;
        }

      
        public Permission CreatOrUpdate(Permission permission)
        {
            var isCreated = _responsitory.Where(p => p.Path == permission.Path).Any();
            if (isCreated)
            {
                return permission;
            }
            var result = _responsitory.InsertOrUpdate(permission);
            return result;
        }

        public Permission Get(string path)
        {
           return _responsitory.Where(p => p.Path == path).First();
        }
    }
}