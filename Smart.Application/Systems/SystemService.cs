using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Smart.Application.Attributes;
using Smart.Application.Systems.Dto;
using Smart.Application.Systems.interfaces;
using Smart.Domain.Entities;
using Smart.Domain.Entities.Enums;
using Smart.Domain.Systems.Interfaces;
using Smart.Infrastructure.Dto;
using Smart.Infrastructure.Freesql.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Panda.DynamicWebApi.Attributes;

namespace Smart.Application.Systems
{
    /// <summary>
    /// 系统服务
    /// </summary>
    [ApiExplorerSettings(GroupName = "Smart")]
    [DynamicWebApi]
    [SMAuthorize]
    public partial class SystemService : BaseService<SystemService>, ISystemService
    {
        ISysDicDetailManager _systemManager;
        ISysDicManager _dicManager;
        IPermissionManager _permissionManager;
        IMapper _mapper;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="systemManager"></param>
        /// <param name="dicManager"></param>
        /// <param name="mapper"></param>
        /// <param name="permissionManager"></param>
        /// <param name="logger"></param>
        public SystemService(ISysDicDetailManager systemManager, ISysDicManager dicManager, IMapper mapper,IPermissionManager permissionManager, ILogger<SystemService> logger) : base(logger)
        {
            _systemManager = systemManager;
            _dicManager = dicManager;
            _permissionManager = permissionManager;
            _mapper = mapper;
        }

       

        /// <summary>
        /// 获取字典项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IResponseOutput Get(long id)
        {
            var model = _systemManager.Get(id);
            var result = _mapper.Map<SysDicDetailOutputDto>(model);
            return new ResponseOutput<SysDicDetailOutputDto>(result);
        }
        /// <summary>
        /// 获取指定TYPE下的所有字典项
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public IResponseOutput GetAll(SysDicDetailInputDto input)
        {
            var result = _systemManager.GetAll(input.PageIndex, input.PageSize, input.Type);
            var dtos = new List<SysDicDetailOutputDto>();
            foreach (var item in result)
            {
                dtos.Add(_mapper.Map<SysDicDetailOutputDto>(item));
            }
            var count = _systemManager.Count(input.Type);
            var output = new SysDicDetailListOutputDto { List = dtos, TotalPageSize = count };
            return new ResponseOutput<SysDicDetailListOutputDto>(output);
        }
        /// <summary>
        /// 获取指定类型的字典
        /// </summary>
        /// <param name="code">类型编码</param>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public IResponseOutput GetList(string code,string key)
        {
            var result = _systemManager.GetAll(code, key);
            var dtos = new List<SysDicDetailOutputDto>();
            foreach (var item in result)
            {
                dtos.Add(_mapper.Map<SysDicDetailOutputDto>(item));
            }
            
            return Ok(dtos);
        }

        /// <summary>
        /// 所有字典
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<SysDicOutputDto> GetAllDic()
        {
            var result = _systemManager.GetAllDic();
            var dtos = new List<SysDicOutputDto>();
            foreach (var item in result)
            {
                dtos.Add(_mapper.Map<SysDicOutputDto>(item));
            }
            return dtos;
        }

        /// <summary>
        /// 字典
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public SysDicOutputDto GetDic(long id)
        {
            var item = _systemManager.GetDic(id);
            var model = _mapper.Map<SysDicOutputDto>(item);
            return model;
        }
        /// <summary>
        /// 根据code获取
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<SysDicDetailOutputDto> GetDicByCode(string code)
        {
            var result = _systemManager.GetDicByCode(code);
            if (result == null)
            {
                return null;
            }
            var dtos = new List<SysDicDetailOutputDto>();
            foreach (var item in result)
            {
                dtos.Add(_mapper.Map<SysDicDetailOutputDto>(item));
            }
            return dtos;
        }

        /// <summary>
        /// 数据字典更新
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IResponseOutput RegisterOrUpdate(SysDicDetailOutputDto dto)
        {
            var item = _systemManager.CreatOrUpdate(_mapper.Map<SysDicDetail>(dto));
            return new ResponseOutput<SysDicDetail>(item);
        }
        /// <summary>
        /// 字典更新
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public IResponseOutput RegisterOrUpdateDic(SysDicOutputDto dic)
        {
            var item = _dicManager.CreatOrUpdate(_mapper.Map<SysDic>(dic));
            return new ResponseOutput<SysDic>(item);
        }
        /// <summary>
        /// 检举转换为json对象
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public IResponseOutput GetSystemEnums()
        {
            var result = EnumHelper.GetEntitiyEnums();
            return Ok(result);
        }
    }
}
