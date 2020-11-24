using Smart.Domain.Entities;
using Smart.Domain.Responsitories;
using Smart.Domain.Users.Interfaces;
using Smart.Infrastructure.Exceptions;
using Smart.Infrastructure.Extensions;
using Smart.Infrastructure.Freesql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Smart.Domain.Users
{
    public class UserManager : IUserManager
    {
        ISysUserResponsitory _responsitory;

        IUserRoleResponsitory _userRoleResponsitory;

        IRolePermissionResponsitory _rolePermissionResponsitory;
        IPermissionResponsitory _permissionResponsitory;
        public UserManager(ISysUserResponsitory responsitory, IUserRoleResponsitory userRoleResponsitory, IRolePermissionResponsitory rolePermissionResponsitory, IPermissionResponsitory permissionResponsitory)
        {
            _responsitory = responsitory;
            _userRoleResponsitory = userRoleResponsitory;
            _rolePermissionResponsitory = rolePermissionResponsitory;
            _permissionResponsitory = permissionResponsitory;
        }

        public IEnumerable<SysUser> GetAll()
        {
            var result = _responsitory.Select.ToList();
            return result;
        }

        public IEnumerable<SysUser> GetAll(int pageIndex, int pageSize)
        {
            var result = _responsitory.Where(p => true).OrderBy(p => p.RealName).ToPageList(pageIndex, pageSize);
            return result;
        }

        public SysUser CheckUser(string userName, string password)
        {
            var user = _responsitory.Where(p => p.UserName == userName && p.Password == password).First();
            return user;
        }

        public SysUser CheckUser(string userName)
        {
            var user = _responsitory.Where(p => p.UserName == userName).First();
            return user;
        }

        public SysUser Get(long id)
        {
            var user = _responsitory.Where(p => p.Id == id).First();
            return user;
        }

        public SysUser Get(string userName)
        {
            var user = _responsitory.Where(p => p.UserName == userName).First();
            return user;
        }

        public long Count()
        {
            var count = _responsitory.Select.Count();
            return count;
        }

        public SysUser CreatOrUpdate(SysUser user)
        {
            var nUser = CheckUser(user.UserName);

            if (user.Id >= 0)
            {
                if (nUser != null && nUser.Id != user.Id)
                {
                    throw new DbException("用户名已存在!");
                }

            }
            else if (nUser != null)
            {
                throw new DbException("用户名已存在!");
            }
            user.Pinyin = user.RealName.ToPinyin();
            user.ShortPinyin = user.RealName.ToPinyinForShort();
            var result = _responsitory.InsertOrUpdate(user);

            return result;
        }

        public IEnumerable<SysUser> GetList(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return _responsitory.Where(p => true).ToList();
            }
            key = key.ToLower();
            var result = _responsitory.Where(p => p.RealName.Contains(key) || p.Pinyin.Contains(key) || p.ShortPinyin.Contains(key))
                 .ToList();
            return result;
        }

        public bool CheckPermission(long userId, string permission, bool checkFather = false)
        {
            var arr = permission.Split('|');
            var path = arr[0];
            var desc = arr[0];
            if (arr.Length > 1)
            {
                desc = arr[1];
            }
            var isPermission = _permissionResponsitory.Where(p => p.Path == path).Any();
            if (!isPermission)
            {
                _permissionResponsitory.Insert(new Permission { Path = path, Description = desc });
                return false;
            }
            //TODO 先从缓存读取权限信息
            var roles = _userRoleResponsitory.Where(p => p.UserId == userId).ToList();
            if (!roles.Any())
            {
                return false;
            }
            var roleIds = roles.Select(p => p.Id).ToList();
            Debug.WriteLine("验证权限：" + path);
            var isCheck = _rolePermissionResponsitory.Where(p => p.Permission.Path == path && roleIds.Contains(p.RoleId)).Any();
            if (isCheck && checkFather)
            {
                var index = path.LastIndexOf('.');
                if (index != -1)
                {
                    path = path.Substring(0, index);
                    Debug.WriteLine("验证权限：" + path);
                    _rolePermissionResponsitory.Where(p => p.Permission.Path == path && roleIds.Contains(p.RoleId)).Any();
                }
            }
            return isCheck;
        }

        public IEnumerable<Permission> GetPermissions(long userId)
        {
            var roles = _userRoleResponsitory.Where(p => p.UserId == userId).ToList();
            if (roles.Any())
            {
                var roleIds = roles.Select(p => p.Id).ToList();
                var list = _rolePermissionResponsitory.Where(p =>roleIds.Contains(p.RoleId)).Include(p=>p.Permission).ToList();
                return list.Select(p=>p.Permission);
            }
            return null;
        }
    }
}
