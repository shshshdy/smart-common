//=============================================================
// 创建人:            ssd
// 创建时间:          2020/9/19 22:44:21
// 邮箱：             1292934053@qq.com
//==============================================================
using Smart.Domain.Entities;

namespace Smart.Domain.Systems.Interfaces
{
    public interface IPermissionManager: IManager<Permission>
    {
        /// <summary>
        /// 权限路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        Permission Get(string path);
    }
}