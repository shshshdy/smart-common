//=============================================================
// 创建人:            ssd
// 创建时间:          2020/9/26 10:24:31
// 邮箱：             1292934053@qq.com
//==============================================================
using Smart.Domain.Files.Interfaces;
using System.Collections.Generic;
using Smart.Infrastructure.Freesql.Entities;
using Smart.Infrastructure.Freesql.Responsitories;

namespace Smart.Domain.Files
{
    public class SysFileManager : ISysFileManager
    {
        ISysFileResponsitory _responsitory;
        public SysFileManager(ISysFileResponsitory responsitory)
        {
            _responsitory = responsitory;
        }

        public IEnumerable<SysFile> GetAll()
        {
            var result = _responsitory.Select.ToList();
            return result;
        }

        public IEnumerable<SysFile> GetAll(int pageIndex, int pageSize)
        {
            var result = _responsitory.Where(p => true).OrderBy(p => p.Id).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToList();
            return result;
        }

        public SysFile Get(long id)
        {
            var user = _responsitory.Where(p => p.Id == id).First();
            return user;
        }

        public long Count()
        {
            var count = _responsitory.Select.Count();
            return count;
        }

      
        public SysFile CreatOrUpdate(SysFile sysFile)
        {
            var result = _responsitory.InsertOrUpdate(sysFile);
            return result;
        }
        public SysFile GetForMd5(string md5)
        {
            var result = _responsitory.Where(p => p.Md5 == md5).First();
            return result;
        }
    }
}