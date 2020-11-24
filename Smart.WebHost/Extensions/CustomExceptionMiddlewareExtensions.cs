using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Smart.Infrastructure.Dto;

namespace Smart.Host.Extensions
{
    /// <summary>
    /// You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    /// </summary>
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionMiddleware> _logger;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                Console.Write(ex.StackTrace);
                
                _logger.LogError("Unhandled exception....", ex);
                var msg = JsonConvert.SerializeObject(new ResponseOutput
                {
                    Success = false,
                    StatusCode = 500,
                    Msg = "未处理异常：" + ex.Message
                }, new JsonSerializerSettings
                {
                    StringEscapeHandling = StringEscapeHandling.EscapeNonAscii,
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                });
                await httpContext.Response.WriteAsync(msg);
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            //TODO 缓存或数据库收集错误信息
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// Extension method used to add the middleware to the HTTP request pipeline.
    /// </summary>
    public static class CustomExceptionMiddlewareExtensions
    {
        /// <summary>
        /// api使用自定义错误信息
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCustomExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionMiddleware>();
        }
    }
}
