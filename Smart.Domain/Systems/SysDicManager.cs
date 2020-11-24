using System.Collections.Generic;
using Smart.Domain.Systems.Interfaces;
using Smart.Infrastructure.Exceptions;
using Smart.Infrastructure.Freesql.Entities;
using Smart.Infrastructure.Freesql.Responsitories;

namespace Smart.Domain.System
{
    public class SysDicManager : ISysDicManager
    {
        ISysDicResponsitory _responsitory;
        public SysDicManager(ISysDicResponsitory responsitory)
        {
            _responsitory = responsitory;
        }

        public IEnumerable<SysDic> GetAll()
        {
            var result = _responsitory.Select.ToList();
            return result;
        }

        public IEnumerable<SysDic> GetAll(int pageIndex, int pageSize)
        {
            var result = _responsitory.Where(p => true).OrderBy(p => p.Id).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToList();
            return result;
        }

        public SysDic Get(long id)
        {
            var user = _responsitory.Where(p => p.Id == id).First();
            return user;
        }

        public long Count()
        {
            var count = _responsitory.Select.Count();
            return count;
        }

        public SysDic CreatOrUpdate(SysDic sysDic)
        {
            var nSys = _responsitory.Where(p => p.Code == sysDic.Code).First();

            if (sysDic.Id >= 0)
            {
                if (nSys != null && nSys.Id != sysDic.Id)
                {
                    throw new DbException("编码已存在!");
                }

            }
            else if (nSys != null)
            {
                throw new DbException("编码已存在!");
            }
            var result = _responsitory.InsertOrUpdate(sysDic);
            return result;
        }

        public SysDic GetDicByCode(string code)
        {
            var result = _responsitory.Where(p => p.Code == code).First();
            return result;
        }
    }
}
