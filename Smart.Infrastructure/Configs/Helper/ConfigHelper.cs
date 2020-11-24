using System;
using System.IO;
using Smart.Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;

namespace Smart.Infrastructure.Configs.Helper
{
    /// <summary>
    /// 配置帮助类
    /// </summary>
    public static class ConfigHelper
    {
        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="environmentName">环境名称</param>
        /// <param name="reloadOnChange">自动更新</param>
        /// <returns></returns>
        private static IConfiguration Load(string fileName, string environmentName = "", string configPath= "Configs", bool reloadOnChange = false)
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, configPath);
            if (!Directory.Exists(filePath))
                return null;

            var builder = new ConfigurationBuilder()
                .SetBasePath(filePath)
                .AddJsonFile(fileName.ToLower() + ".json", true, reloadOnChange);

            if (environmentName.NotNull())
            {
                var json = fileName.ToLower() + "." + environmentName + ".json";
                if (File.Exists(Path.Combine(filePath, json)))
                {
                    builder.AddJsonFile(json, true, reloadOnChange);
                }
                else
                {
                    builder.AddJsonFile(fileName.ToLower() + ".json", true, reloadOnChange);
                }

            }

            return builder.Build();
        }

        /// <summary>
        /// 获得configPath下的 T.json转为T对象
        /// </summary>
        /// <typeparam name="T">配置对象</typeparam>
        /// <param name="environmentName">开发环境名</param>
        /// <param name="configPath">文件名称</param>
        /// <param name="reloadOnChange">自动更新</param>
        /// <returns></returns>
        public static T Get<T>(string environmentName = "", string configPath = "Configs", bool reloadOnChange = false)
        {
            var configuration = Load(typeof(T).Name.ToLower(), environmentName, configPath, reloadOnChange);
            if (configuration == null)
                return default;

            return configuration.Get<T>();
        }
    }
}
