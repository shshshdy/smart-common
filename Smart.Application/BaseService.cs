using Smart.Shared.Dto;
using Microsoft.Extensions.Logging;
using System;

namespace Smart.Application
{
    /// <summary>
    /// BaseService
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseService<T>
    {
        /// <summary>
        /// logger
        /// </summary>
        protected readonly ILogger<T> _logger;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public BaseService(ILogger<T> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <param name="showLog">是否显示日志</param>
        /// <returns></returns>
        protected IResponseOutput Error(string error, bool showLog = false)
        {
            if (showLog)
            {
                var time = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                _logger.LogWarning($"time:{time} {error}");
            }
            return new ResponseOutput { Success = false, StatusCode = -1, Msg = error };
        }
        /// <summary>
        /// 成功
        /// </summary>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="output"></param>
        /// <returns></returns>
        protected IResponseOutput Ok<TOutput>(TOutput output)
        {
            return new ResponseOutput<TOutput>(output);
        }
    }
}
