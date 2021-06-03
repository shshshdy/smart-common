using AutoMapper;
using Smart.Shared.Files.Dto;
using Smart.Shared.Systems.Dto;
using Smart.Shared.Users.Dto;
using Smart.Domain.Entities;
using Smart.Infrastructure.Freesql.Entities;

namespace Smart.Application
{
    /// <summary>
    /// automap配置
    /// </summary>
    public class AutoMapperConfigs : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public AutoMapperConfigs()
        {
            CreateMap<SysUser, UserDto>();
            CreateMap<UserDto, SysUser>();
            CreateMap<SysUser, AuthenticateDto>();
            CreateMap<SysUser, UserOutputDto>();
            CreateMap<SysUser, UserSearchOutputDto>();

            CreateMap<SysDicDetail, SysDicDetailOutputDto>().ForMember(p => p.DicName, f => f.MapFrom(s => s.SysDic.Name));
            CreateMap<SysDic, SysDicOutputDto>();
            CreateMap<SysDicDetailOutputDto, SysDicDetail>();
            CreateMap<SysDicOutputDto, SysDic>();

            CreateMap<SysFile, SysFileOutputDto>();

        }
    }
}
