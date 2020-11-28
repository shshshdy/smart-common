using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart.Infrastructure.Configs
{
    /// <summary>
    /// 应用配置
    /// </summary>
    public class AppConfig
    {
        /// <summary>
        /// 应用名称
        /// </summary>
        public string Name { get; set; } = "BaseSmart";
        /// <summary>
        /// 默认密码
        /// </summary>
        public string DefaultPwd { get; set; }

        /// <summary>
        /// Swagger文档
        /// </summary>
        public bool Swagger { get; set; }

        /// <summary>
        /// Api调用地址，多地址|分隔开，默认 http://*:8080
        /// </summary>
        public string Urls { get; set; } = "http://*:8080";

        /// <summary>
        /// 日志配置
        /// </summary>
        public LogConfig Log { get; set; }

        /// <summary>
        /// 文件存放路径
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        ///目录访问身份
        ///windows下共享文件夹,liunx下可采用挂载方式
        /// </summary>
        public string FileAuth { get; set; }
        /// <summary>
        ///目录访问密码
        /// </summary>
        public string FilePwd { get; set; }
    }
}
