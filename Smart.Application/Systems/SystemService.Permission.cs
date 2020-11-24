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
        /// <param name="Name"></param>
        /// <returns></returns>
        [SMAuthorize(Consts.SystemUser)]
        public bool CheckPermission(string path,string Name)
        {
            
            return false;
        }
        /// <summary>
        /// 创建所有权限
        /// </summary>
        public void CreatAllPermissions()
        {
            var fis = typeof(Consts).GetFields();
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
