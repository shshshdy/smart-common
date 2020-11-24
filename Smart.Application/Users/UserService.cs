using AutoMapper;
using Smart.Application.Attributes;
using Smart.Application.Dtos;
using Smart.Application.Users.Dto;
using Smart.Application.Users.interfaces;
using Smart.Domain.Entities;
using Smart.Domain.Users.Interfaces;
using Smart.Infrastructure.Authenticate;
using Smart.Infrastructure.Configs;
using Smart.Infrastructure.Dto;
using Smart.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Panda.DynamicWebApi.Attributes;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Smart.Application.Users
{
    /// <summary>
    /// 用户服务
    /// </summary>
    [SMAuthorize]
    [DynamicWebApi]
    public class UserService : BaseService<UserService>, IUserService
    {

        IUserManager _userManager;
        IMapper _mapper;
        AppConfig _appConfig;
        JwtConfig _tokenManage;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="appConfig"></param>
        /// <param name="tokenManage"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public UserService(IUserManager userManager, AppConfig appConfig, JwtConfig tokenManage, IMapper mapper, ILogger<UserService> logger) : base(logger)
        {
            _userManager = userManager;
            _mapper = mapper;
            _appConfig = appConfig;
            _tokenManage = tokenManage;
        }


        /// <summary>
        /// 注册或修改用户
        /// </summary>
        /// <returns></returns>
        public IResponseOutput RegisterOrUpdate(UserDto user)
        {
            user.UserName = user.UserName?.Trim();
            user.Password = user.Password?.Trim();
            if (string.IsNullOrEmpty(user.UserName))
            {
                return Error("用户为空!");
            }
            if (user.Id == 0 && string.IsNullOrEmpty(user.Password))
            {
                return Error("密码为空!");
            }

            var model = _mapper.Map<SysUser>(user);
            var result = _userManager.CreatOrUpdate(model);
            return new ResponseOutput<SysUser>(result);
        }

        /// <summary>
        /// 查询所有用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseOutput GetAll(InputDto input)
        {
            var result = _userManager.GetAll(input.PageIndex, input.PageSize);
            IList<UserOutputDto> dtos = new List<UserOutputDto>();
            foreach (var item in result)
            {
                dtos.Add(_mapper.Map<UserOutputDto>(item));
            }
            var count = _userManager.Count();
            var output = new UserListOutputDto { List = dtos, TotalPageSize = count };
            return new ResponseOutput<UserListOutputDto>(output);
        }
        /// <summary>
        /// 根据用户名ID获取用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IResponseOutput Get(long id)
        {
            var user = _userManager.Get(id);
            var result = _mapper.Map<UserOutputDto>(user);
            return new ResponseOutput<UserOutputDto>(result);
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public IResponseOutput GetList(string key)
        {
            var list = _userManager.GetList(key);
            var result = _mapper.Map<List<UserSearchOutputDto>>(list);
            return Ok(result);
        }
       
        /// <summary>
        /// 初始密码
        /// </summary>
        /// <returns></returns>
        public IResponseOutput GetDefaultPassword()
        {
            return new ResponseOutput<string>(_appConfig.DefaultPwd);
        }

        /// <summary>
        /// 验证用户信息，并返回权限等资料
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private AuthenticateDto IsValid(UserInputDto user)
        {
            var nUser = _userManager.CheckUser(user.UserName, user.Password.PwdEncode());

            if (nUser != null)
            {
                var output = _mapper.Map<AuthenticateDto>(nUser);
                output.IsAuthenticated = true;
                var list = _userManager.GetPermissions(nUser.Id);
                if (list != null)
                {
                    output.Permissions = list.Select(p => p.Path).ToArray();
                }
                return output;
            }

            return new AuthenticateDto { IsAuthenticated = false };
        }
        [AllowAnonymous]
        public IResponseOutput Login(UserInputDto request)
        {
            var user = IsValid(request);
            if (!user.IsAuthenticated)
            {
                return Error("用户名或密码错误", true);
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimAttributes.UserId, user.Id.ToString()),
                new Claim(ClaimAttributes.UserName, user.UserName),
                new Claim(ClaimAttributes.UserRealName, user.RealName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManage.SecurityKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(_tokenManage.Issuer, _tokenManage.Audience, claims, expires: DateTime.Now.AddMinutes(_tokenManage.Expires), signingCredentials: credentials);

            user.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return Ok(user);
        }
    }
}