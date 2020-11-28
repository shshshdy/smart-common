using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Smart.Application
{
    /// <summary>
    /// 权限，定义方式为 "key|description"
    /// </summary>
    public class Permissions
    {
        /// <summary>
        /// 系统管理
        /// </summary>
        public const string System = "system|系统管理";
        /// <summary>
        /// 用户管理
        /// </summary>
        public const string SystemUser = "system.user|用户管理";
    }
}
