using System.Collections.Generic;
using Smart.Domain.Entities;
using Smart.Domain.Responsitories;
using Smart.Domain.Systems.Interfaces;
using Smart.Infrastructure.Exceptions;
using Smart.Infrastructure.Freesql;
using Smart.Infrastructure.Freesql.Entities;
using Smart.Infrastructure.Freesql.Responsitories;

namespace Smart.Domain.Systems
{
    public class SysDicDetailManager : ISysDicDetailManager
    {
        ISysDicResponsitory _dicResponsitory;
        ISysDicDetailResponsitory _responsitory;
        public SysDicDetailManager(ISysDicResponsitory dicResponsitory, ISysDicDetailResponsitory responsitory)
        {
            _dicResponsitory = dicResponsitory;
            _responsitory = responsitory;
        }

        public long Count()
        {
            var count = _responsitory.Select.Count();
            return count;
        }

        public long Count(long type)
        {
            if (type == 0)
            {
                return Count();
            }
            var count = _responsitory.Select.Where(p => p.DicId == type).Count();
            return count;
        }

        public SysDicDetail CreatOrUpdate(SysDicDetail entity)
        {
            var nModel = _responsitory.Where(p => p.DicId == entity.DicId && p.Value == entity.Value).First();
            if (entity.Id > 0)
            {
                if (nModel != null && nModel.Id != entity.Id)
                {
                    throw new DbException("值已存在!");
                }
            }
            else if (nModel != null)
            {
                throw new DbException("值已存在!");
            }

            var result = _responsitory.InsertOrUpdate(entity);
            return result;
        }

        public SysDicDetail Get(long id)
        {
            var result = _responsitory.Find(id);
            return result;
        }

        public IEnumerable<SysDicDetail> GetAll()
        {
            var result = _responsitory.Where(p => p.Enabled).ToList();
            return result;
        }

        public IEnumerable<SysDicDetail> GetAll(int pageIndex, int pageSize)
        {
            var result = _responsitory
                .Where(p => p.Enabled)
                .Include(p => p.SysDic)
                .OrderBy(p => p.DicId).OrderBy(p => p.Order)
                .ToPageList(pageIndex,pageSize);
            return result;
        }

        public IEnumerable<SysDicDetail> GetAll(int pageIndex, int pageSize, long type)
        {
            if (type == 0)
            {
                return GetAll(pageIndex, pageSize);
            }
            var result = _responsitory
                .Where(p => p.DicId == type && p.Enabled)
                .Include(p => p.SysDic)
                .OrderBy(p => p.DicId).OrderBy(p => p.Order)
                .ToPageList(pageIndex, pageSize);
            return result;
        }

        public IEnumerable<SysDicDetail> GetAll(string typeCode, string key)
        {
            var select = _responsitory
               .Where(p => p.SysDic.Code == typeCode && p.Enabled);
            if (!string.IsNullOrEmpty(key))
            {
                select = select.Where(p => p.Key.Contains(key));
            }
            var result = select.OrderBy(p => p.Order).ToList();
            return result;
        }

        public IEnumerable<SysDic> GetAllDic()
        {
            var result = _dicResponsitory.Select.ToList();
            return result;
        }

        public SysDic GetDic(long id)
        {
            var result = _dicResponsitory.Find(id);
            return result;
        }

        public IEnumerable<SysDicDetail> GetDicByCode(string code)
        {
            var dic = _dicResponsitory.Where(p => p.Code == code).First();
            if (dic == null)
            {
                return null;
            }
            var result = _responsitory.Where(p => p.DicId == dic.Id && p.Enabled).ToList();
            return result;
        }
    }
}
