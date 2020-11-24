//=============================================================
// 创建人:            ssd
// 创建时间:          2020/9/5 18:27:05
// 邮箱：             1292934053@qq.com
//==============================================================
using Smart.Domain.Entities;
using Smart.Domain.Entities.Enums;
using Smart.Domain.Responsitories;
using Smart.Domain.Systems.Interfaces;
using System;
using System.Collections.Generic;

namespace Smart.Domain.Systems
{
    public class SysCodeManager : ISysCodeManager
    {
        readonly ISysCodeResponsitory _responsitory;
        public SysCodeManager(ISysCodeResponsitory responsitory)
        {
            _responsitory = responsitory;
        }

        public IEnumerable<SysCode> GetAll()
        {
            var result = _responsitory.Select.ToList();
            return result;
        }

        public IEnumerable<SysCode> GetAll(int pageIndex, int pageSize)
        {
            var result = _responsitory.Where(p => true).OrderBy(p => p.Id).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToList();
            return result;
        }

        public SysCode Get(long id)
        {
            var user = _responsitory.Where(p => p.Id == id).First();
            return user;
        }

        public long Count()
        {
            var count = _responsitory.Select.Count();
            return count;
        }


        public SysCode CreatOrUpdate(SysCode sysCode)
        {
            var result = _responsitory.InsertOrUpdate(sysCode);
            return result;
        }

        public string NextCode(string prefix)
        {
            var code = _responsitory.Where(p => p.Prefix == prefix && p.CurrentDate.Date == DateTime.Today.Date).First();
            if (code == null)
            {
                CreatOrUpdate(new SysCode { Prefix = prefix, CurrentDate = DateTime.Now, Code = 1 });
                return $"{prefix.ToString().ToUpper()}{DateTime.Now:yyyy-MM-dd}001";
            }
            else
            {
                code.Code += 1;
                CreatOrUpdate(code);
                return $"{prefix.ToString().ToUpper()}{DateTime.Now:yyyy-MM-dd}{code.Code:D3}";
            }

        }
    }
}