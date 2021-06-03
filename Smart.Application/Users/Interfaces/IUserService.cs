using Smart.Shared.Dtos;
using Smart.Application.Interfaces;
using Smart.Shared.Users.Dto;
using Smart.Shared.Dto;

namespace Smart.Application.Users.interfaces
{
    /// <summary>
    /// 用户服务
    /// </summary>
    public interface IUserService : IService<InputDto, long>
    {
        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        IResponseOutput RegisterOrUpdate(UserDto user);

    }
}
