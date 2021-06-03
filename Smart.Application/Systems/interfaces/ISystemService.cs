using System;
using System.Collections.Generic;
using Smart.Application.Interfaces;
using Smart.Shared.Systems.Dto;
using Smart.Shared.Dto;

namespace Smart.Application.Systems.interfaces
{
    /// <summary>
    /// 系统service
    /// </summary>
    public interface ISystemService : IService<SysDicDetailInputDto, long>
    {
        /// <summary>
        /// 添加或更新
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        IResponseOutput RegisterOrUpdate(SysDicDetailOutputDto detail);

        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        IEnumerable<SysDicOutputDto> GetAllDic();

        /// <summary>
        /// 获取单条
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        SysDicOutputDto GetDic(long id);

        /// <summary>
        /// 更新字典
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        IResponseOutput RegisterOrUpdateDic(SysDicOutputDto dic);
        /// <summary>
        /// 创建所有权限
        /// </summary>
        void CreatAllPermissions(Type type);
        /// <summary>
        /// 获取字典
        /// </summary>
        /// <param name="code">字典类型编号</param>
        /// <returns></returns>
        IEnumerable<SysDicDetailOutputDto> GetDicByCode(string code);
    }
}
