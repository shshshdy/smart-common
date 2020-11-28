using System.Diagnostics;
using Smart.Host.Models;
using Microsoft.AspNetCore.Mvc;
using Smart.Infrastructure.Configs;

namespace Smart.Host.Controllers
{

    /// <summary>
    /// Home
    /// </summary>
    public class HomeController : Controller
    {
        AppConfig _appConfig;
        /// <summary>
        /// Home
        /// </summary>
        /// <param name="appConfig"></param>
        public HomeController(AppConfig appConfig)
        {
            _appConfig = appConfig;
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
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
    }
}
