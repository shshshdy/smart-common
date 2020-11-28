using System;
using System.IO;
using RazorEngine;
using RazorEngine.Templating;
using Freesql.Tool.Helpers;
using Freesql.Tool.Models;
using System.Collections.Generic;

namespace Freesql.Tool
{
    class MainClass
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                foreach(var item in args)
                {
                    if (QuickStart(item))
                    {
                        Console.WriteLine("恭喜成功生成代码，按任意键结束!");
                    }
                }
            }
            else
            {
                var config = Environment.OSVersion.Platform == PlatformID.Win32NT ? "Config.Win.txt" : "Config.txt";
                if (QuickStart(config))
                {
                    Console.WriteLine("恭喜成功生成代码，按任意键结束!");
                }
            }
            Console.ReadKey();
        }

        private static bool QuickStart(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine($"配置文件 {path} 不存在!");
                return false;
            }
            var rootPath = Directory.GetParent(path).FullName;
            var lines = File.ReadAllLines(path);
            var model = new RazorTemplateModel
            {
                UserInfo = new CopyRightUserInfo
                {
                    CreateTime = DateTime.Now
                }
            };
            var tables = "";
            var templates = new List<string>();
            foreach(var item in lines)
            {
                if(string.IsNullOrEmpty(item) || item.StartsWith(";"))
                {
                    continue;
                }
                if (item.StartsWith("-Author"))
                {
                    model.UserInfo.Author = item.Replace("-Author", "").Trim();
                }
                else if (item.StartsWith("-Email"))
                {
                    model.UserInfo.Email = item.Replace("-Email", "").Trim();
                }
                else if (item.StartsWith("-NameSpace"))
                {
                    model.NameSpace = item.Replace("-NameSpace", "").Trim();
                }
                else if (item.StartsWith("-Folder"))
                {
                    model.Folder = item.Replace("-Folder", "").Trim();
                }
                else if (item.StartsWith("-Assmbly"))
                {
                    model.AssemblyOrConnect = item.Replace("-Assmbly", "").Trim();
                }
                else if (item.StartsWith("-TableNames"))
                {
                    tables = item.Replace("-TableNames", "").Trim();
                }
                else if (item.StartsWith("-Out"))
                {
                    model.OutPath = item.Replace("-Out", "").Trim();
                }
                else if (item.StartsWith("-T"))
                {
                    var template = item.Replace("-T", "").Trim();
                    if (!string.IsNullOrEmpty(template))
                    {
                        templates.Add(template);
                    }
                }
            }
            if (string.IsNullOrEmpty(tables) || templates.Count == 0)
            {
                Console.WriteLine("无需处理!");
                return false;
            }
            if (model.AssemblyOrConnect.ToLower().IndexOf(".dll") != -1 && !File.Exists(model.AssemblyOrConnect))
            {
                Console.WriteLine("程序集不存在!");
                return false;
            }
            foreach (var item in tables.Split(','))
            {
                Console.WriteLine($"======{item}======");
                foreach(var template in templates)
                {
                    model.TableName = item.ToFirstUp();
                    var tPath = template
                        .Replace("{NameSpace}", model.NameSpace)
                        .Replace("{TableName}", model.TableName)
                        .Replace("{Folder}", model.Folder)
                        .Replace("{JsName}",model.TableName.ToJsName())
                        .Replace("{VuePath}",model.TableName.ToVuePath());
                    var paths = tPath.Split(',');
                    if (paths.Length > 1)
                    {
                        model.UserInfo.CreateTime = DateTime.Now;
                        model.Folder = string.IsNullOrEmpty(model.Folder) ? model.TableName.ToPlural() : model.Folder;
                        paths[1] = Path.Combine(model.OutPath, paths[1]);
                        var inPath = IsAbsolute(paths[0]) ? paths[0] : Path.Combine(rootPath, paths[0]);
                        var outPath = IsAbsolute(paths[1]) ? paths[1] : Path.Combine(rootPath, paths[1]);
                        OutTemplate(inPath, outPath, model);
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 是否绝对路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static bool IsAbsolute(string path)
        {
            return path.StartsWith('/') || path.Length > 1 && path.Substring(1, 1) == ":";
        }
        /// <summary>
        /// 模板输出对应文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="outPath"></param>
        /// <param name="model"></param>
        private static void OutTemplate(string path, string outPath, RazorTemplateModel model=null)
        {
            var template = File.ReadAllText(path);
            var result = Engine.Razor.RunCompile(template, Guid.NewGuid().ToString(), typeof(RazorTemplateModel), model);
            var dir = Path.GetDirectoryName(outPath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            File.WriteAllText(outPath, result);
            Console.WriteLine($"{outPath} done!");
        }
    }
}
