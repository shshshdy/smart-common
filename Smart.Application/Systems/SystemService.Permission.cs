using Smart.Application.Attributes;
using Smart.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Smart.Application.Systems
{
    public partial class SystemService
    {
        /// <summary>
        /// 检查权限
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [SMAuthorize(Permissions.SystemUser)]
        public bool CheckPermission(string path)
        {
            var result = _permissionManager.Get(path);
            return result != null;
        }
        /// <summary>
        /// 创建所有权限
        /// </summary>
        public void CreatAllPermissions(Type type)
        {
            CreatePermissions(type);
            while (type.BaseType != null)
            {
                type = type.BaseType;
                CreatePermissions(type);
            }
        }

        private void CreatePermissions(Type type)
        {
            var fis = type.GetFields();
            foreach (var fieldInfo in fis)
            {
                var value = fieldInfo.GetRawConstantValue().ToString();
                var arr = value.Split('|');
                var path = arr[0];
                var desc = arr[0];
                if (arr.Length > 1)
                {
                    desc = arr[1];
                }
                _permissionManager.CreatOrUpdate(new Permission { Path = path, Description = desc });
            }
        }
    }
}
