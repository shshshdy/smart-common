using Smart.Domain.Users.Interfaces;
using Smart.Infrastructure.Authenticate;
using Smart.Infrastructure.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Smart.Application.Attributes
{
    /// <summary>
    /// 身份认证
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class SMAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter, IAsyncAuthorizationFilter
    {
        string _permission = null;
        bool _canParent = false;
        /// <summary>
        /// 
        /// </summary>
        public SMAuthorizeAttribute()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="permission"></param>
        /// <param name="canParent"></param>
        public SMAuthorizeAttribute(string permission, bool canParent = false) : base()
        {
            _permission = permission;
            _canParent = canParent;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            OnAuthorization(context);
            return Task.CompletedTask;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //不判断权限
            if (string.IsNullOrEmpty(_permission))
            {
                return;
            }
            //排除匿名访问
            if (context.ActionDescriptor.EndpointMetadata.Any(m => m.GetType() == typeof(AllowAnonymousAttribute)))
            {
                return;
            }
            var user = GetManager<IUser>(context) as IUser;
            var permission = GetManager<IUserManager>(context) as IUserManager;
            var isCheck = permission.CheckPermission(user.Id, _permission, _canParent);
            if (!isCheck)
            {
                context.Result = new JsonResult(new ResponseOutput
                {
                    Success = false,
                    StatusCode = 403,
                    Msg = "您无此权限！",
                });
            }
        }

        private static object GetManager<T>(AuthorizationFilterContext context)
        {
            return context.HttpContext.RequestServices.GetService(typeof(T));
        }
    }
}
