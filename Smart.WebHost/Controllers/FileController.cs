using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Smart.Domain.Files.Interfaces;
using Smart.Host.Helpers;
using Smart.Infrastructure.Configs;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using IoFile = System.IO.File;
namespace Smart.Host.Controllers
{
    /// <summary>
    /// 文件服务
    /// </summary>
    [ApiExplorerSettings(GroupName = "Smart")]
    public class FileController: Controller
    {
        private readonly ILogger<FileController> _logger;
        AppConfig _appConfig;
        ISysFileManager _sysFileManager;
        JwtConfig _jwtConfig;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysFileManager"></param>
        /// <param name="jwtConfig"></param>
        /// <param name="logger"></param>
        /// <param name="appConfig"></param>
        public FileController(ISysFileManager sysFileManager, JwtConfig jwtConfig, ILogger<FileController> logger, AppConfig appConfig)
        {
            _logger = logger;
            _appConfig = appConfig;
            _sysFileManager = sysFileManager;
            _jwtConfig = jwtConfig;
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="md5"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/SysFileService/download")]
        public IActionResult Download(string md5, string token)
        {
            if (!CheckReferer())
            {
                _logger.LogError($"非安全域名访问：{GetHost()}");
                return new StatusCodeResult((int)HttpStatusCode.NotAcceptable);
            }

            var jwthandle = new JwtSecurityTokenHandler();
            if (string.IsNullOrEmpty(token) || !jwthandle.CanReadToken(token))
            {
                _logger.LogInformation($"非法下载：{md5}!");
                return new StatusCodeResult((int)HttpStatusCode.NonAuthoritativeInformation);
            }

            var paramters = JwtHelper.GetParameters(_jwtConfig);
            jwthandle.ValidateToken(token, paramters, out SecurityToken sToekn);
            if (sToekn == null)
            {
                _logger.LogInformation($"非法下载：{md5}!");
                return new StatusCodeResult((int)HttpStatusCode.NonAuthoritativeInformation);
            }

            var file = _sysFileManager.GetForMd5(md5);
            if (file == null)
            {
                _logger.LogError($"文件不存在：{md5}!");
                return new StatusCodeResult((int)HttpStatusCode.NotFound);
            }
            var path = Path.Combine(_appConfig.FilePath, file.FileDate, file.CreatedUserId.ToString(), $"{md5}{Path.GetExtension(file.FileName)}");
            if (!IoFile.Exists(path))
            {
                _logger.LogError($"文件不存在：{md5}!");
                return new StatusCodeResult((int)HttpStatusCode.NotFound);
            }
            new FileExtensionContentTypeProvider().Mappings.TryGetValue(Path.GetExtension(path), out var contenttype);
            var stream = IoFile.OpenRead(path);
            return File(stream, contenttype ?? "application/octet-stream", file.FileName);
        }
        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="md5"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/SysFileService/download2")]
        public IActionResult Download(string md5)
        {
            if (!CheckReferer())
            {
                _logger.LogError($"非安全域名访问：{GetHost()}");
                return new StatusCodeResult((int)HttpStatusCode.NotAcceptable);
            }
             var file = _sysFileManager.GetForMd5(md5);
            if (file == null)
            {
                _logger.LogError($"文件不存在：{md5}!");
                return new StatusCodeResult((int)HttpStatusCode.NotFound);
            }
            var path = Path.Combine(_appConfig.FilePath, file.FileDate, file.CreatedUserId.ToString(), $"{md5}{Path.GetExtension(file.FileName)}");
            if (!IoFile.Exists(path))
            {
                _logger.LogError($"文件不存在：{md5}!");
                return new StatusCodeResult((int)HttpStatusCode.NotFound);
            }
            new FileExtensionContentTypeProvider().Mappings.TryGetValue(Path.GetExtension(path), out var contenttype);
            var stream = IoFile.OpenRead(path);
            return File(stream, contenttype ?? "application/octet-stream", file.FileName);
        }

        private bool CheckReferer()
        {
            var host = GetHost();
            if (string.IsNullOrEmpty(host))
            {
                return _appConfig.NullReferer;
            }
            return _appConfig.Urls.Split("|").Any(p => p.StartsWith(host));
        }
        private string GetHost()
        {
            if (!Request.Headers.ContainsKey("Referer"))
            {
                _logger.LogError($"Referer：为空!");
                return null;
            }
            var referer = Request.Headers["Referer"];
            var url = new Uri(referer);
            return $"{url.Scheme}://{url.Host}";
        }

    }
}
