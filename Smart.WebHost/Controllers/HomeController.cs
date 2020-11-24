using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net;
using Smart.Domain.Files.Interfaces;
using Smart.Host.Helpers;
using Smart.Host.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using IoFile = System.IO.File;
using Smart.Infrastructure.Configs;

namespace Smart.Host.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
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
        public HomeController(ISysFileManager sysFileManager, JwtConfig jwtConfig, ILogger<HomeController> logger, AppConfig appConfig)
        {
            _logger = logger;
            _appConfig = appConfig;
            _sysFileManager = sysFileManager;
            _jwtConfig = jwtConfig;
        }

        public IActionResult Index()
        {
            if (_appConfig.Swagger)
            {
                return Redirect("/swagger");
            }
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="md5"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/sysFileService/download")]
        public IActionResult Download(string md5, string token)
        {
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

    }
}
