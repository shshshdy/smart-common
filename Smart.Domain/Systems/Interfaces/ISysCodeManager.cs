//=============================================================
// 创建人:            ssd
// 创建时间:          2020/9/5 18:27:06
// 邮箱：             1292934053@qq.com
//==============================================================
using Smart.Domain.Entities;
namespace Smart.Domain.Systems.Interfaces
{
    public interface ISysCodeManager: IManager<SysCode>
    {
        string NextCode(string pref);
    }
}