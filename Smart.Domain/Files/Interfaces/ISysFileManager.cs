//=============================================================
// 创建人:            ssd
// 创建时间:          2020/9/26 10:24:31
// 邮箱：             1292934053@qq.com
//==============================================================
using Smart.Infrastructure.Freesql.Entities;

namespace Smart.Domain.Files.Interfaces
{
    public interface ISysFileManager: IManager<SysFile>
    {
        SysFile GetForMd5(string md5);
    }
}