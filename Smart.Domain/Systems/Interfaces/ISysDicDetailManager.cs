
using Smart.Domain.Entities;
using Smart.Infrastructure.Freesql.Entities;
using System.Collections.Generic;

namespace Smart.Domain.Systems.Interfaces
{
    public interface ISysDicDetailManager: IManager<SysDicDetail>
    {
        IEnumerable<SysDicDetail> GetAll(int pageIndex, int pageSize, long type);

        IEnumerable<SysDic> GetAllDic();

        SysDic GetDic(long id);

        long Count(long type);
        IEnumerable<SysDicDetail> GetDicByCode(string code);
        /// <summary>
        /// 类型code和关键字查询
        /// </summary>
        /// <param name="typeCode"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        IEnumerable<SysDicDetail> GetAll(string typeCode, string key);
    }
}
