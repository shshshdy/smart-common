using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Smart.Infrastructure.Authenticate;
using Smart.Infrastructure.Freesql.Attributes;
using Smart.Infrastructure.Freesql.Entities;
using Smart.Infrastructure.Freesql.Responsitories;
using FreeSql;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Smart.Infrastructure.Configs;

namespace Smart.Infrastructure.Freesql
{
    /// <summary>
    /// freesql扩展
    /// </summary>
    public static class FreeSqlExtensions
    {
        static IServiceCollection _services;
        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="select"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static IEnumerable<TEntity> ToPageList<TEntity>(this ISelect<TEntity> select, int pageIndex, int pageSize)
            where TEntity : class
        {
            return select.Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToList();
        }

        /// <summary>
        /// 添加数据库配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="appConfig"></param>
        public static void AddFreesql(this IServiceCollection services, AppConfig appConfig, DbConfig dbConfig)
        {
            _services = services;
            //创建数据库
            if (dbConfig.CreateDb)
            {
                //await DbHelper.CreateDatabase(dbConfig);
            }

            #region FreeSql
            var freeSqlBuilder = new FreeSqlBuilder()
                    .UseConnectionString(dbConfig.Type, dbConfig.ConnectionString)
                    .UseAutoSyncStructure(dbConfig.SyncStructure)
                    .UseLazyLoading(false)
                    .UseNoneCommandParameter(true);

            #region 监听所有命令
            if (dbConfig.MonitorCommand)
            {
                freeSqlBuilder.UseMonitorCommand(cmd => { }, (cmd, traceLog) =>
                {
                    //Console.WriteLine($"{cmd.CommandText}\n{traceLog}\r\n");
                    Console.WriteLine($"{cmd.CommandText}\r\n");
                });
            }
            #endregion

            var fsql = freeSqlBuilder.Build();


            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IUser, User>();
            services.AddFreeRepository(filter => filter.Apply<IEntitySoftDelete>("SoftDelete", a => a.IsDeleted == false));

            services.AddScoped<UnitOfWorkManager>();
            services.AddSingleton(fsql);

            services.AddSingleton<ISysFileResponsitory, SysFileResponsitory>();
            services.AddSingleton<ISysDicResponsitory, SysDicResponsitory>();
            #region 初始化数据库

            //同步数据
            if (dbConfig.SyncData)
            {
                
                //await DbHelper.SyncData(fsql, dbConfig);
            }
            #endregion

            //生成数据包
            if (dbConfig.GenerateData && !dbConfig.CreateDb && !dbConfig.SyncData)
            {
                //await DbHelper.GenerateSimpleJsonData(fsql);
            }
            List<string> _md5s = new List<string>();

            #region 监听Curd操作
            if (dbConfig.Curd)
            {
                fsql.Aop.CurdBefore += (s, e) =>
                {
                    Console.WriteLine($"{e.Sql}\r\n");
                };
            }

            fsql.Aop.CurdAfter += (s, e) =>
            {
                if (e.CurdType == FreeSql.Aop.CurdType.Insert || e.CurdType == FreeSql.Aop.CurdType.Update)
                {
                    //文件，更新为已使用
                    if (e.EntityType!=typeof(SysFile) && _md5s.Any())
                    {
                        var file = services.BuildServiceProvider().GetService<ISysFileResponsitory>();
                        file.UpdateDiy.Set(p => p.Used, true).Where(p => _md5s.Contains(p.Md5) && p.Uploaded && !p.Used).ExecuteAffrows();
                        _md5s.Clear();

                    }
                }
                if (e.ElapsedMilliseconds > 3000)
                {
                    //TODO 慢SQL提醒开发者
                    Console.WriteLine($"慢Sql:{e.Sql}\r\n");
                }
            };
            #endregion

            #region 审计数据

            fsql.Aop.AuditValue += (s, e) =>
            {
                if (e.AuditValueType == FreeSql.Aop.AuditValueType.Insert || e.AuditValueType == FreeSql.Aop.AuditValueType.Update)
                {
                    //更新文件为已使用
                    if (e.Property.GetCustomAttributes(typeof(FileAttribute), false).Any())
                    {
                        if (e.Value != null && !_md5s.Any(p => p == e.Value.ToString()))
                        {
                            _md5s.Add(e.Value.ToString());
                        }
                    }

                    var user = services.BuildServiceProvider().GetService<IUser>();
                    if (user == null || !(user.Id > 0))
                    {
                        return;
                    }
                    if (e.AuditValueType == FreeSql.Aop.AuditValueType.Insert)
                    {
                        switch (e.Property.Name)
                        {
                            case "CreatedUserId":
                                e.Value = user.Id;
                                break;
                            case "CreatedUserName":
                                e.Value = user.RealName;
                                break;
                            case "CreatedTime":
                                e.Value = DateTime.Now;
                                break;
                        }
                    }
                    switch (e.Property.Name)
                    {
                        case "ModifiedUserId":
                            e.Value = user.Id;
                            break;
                        case "ModifiedUserName":
                            e.Value = user.Name;
                            break;
                        case "ModifiedTime":
                            e.Value = DateTime.Now;
                            break;
                    }

                }


            };
            #endregion

            #endregion
        }
        ///<summary>
        ///字典左连接查询 使用，NavigateDicAttribute标记属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="select"></param>
        /// <returns></returns>
        public static ISelect<T> LeftJonDic<T>(this ISelect<T> select)
            where T : class
        {
            var responsitory = _services.BuildServiceProvider().GetService(typeof(ISysDicResponsitory)) as ISysDicResponsitory;
            var type = typeof(T);
            foreach (var item in type.GetProperties())
            {
                var navDic = item.GetCustomAttribute(typeof(NavigateDicAttribute), false);
                if (navDic != null)
                {
                    var dicAttr = navDic as NavigateDicAttribute;
                    var dicIdValue = responsitory.Where(p => p.Code == dicAttr.DicCode).First().Id;

                    var target = Expression.Parameter(typeof(T), "p");

                    var dic = Expression.Property(target, item.Name);

                    var left = Expression.Property(dic, "DicId");
                    var right = Expression.Constant(dicIdValue);
                    var body = Expression.Equal(left, right);


                    var left1 = Expression.Property(dic, "Value");
                    var right1 = Expression.Property(target, dicAttr.FieldName);
                    var body1 = Expression.Equal(left1, right1);
                    var expre = Expression.AndAlso(body1, body);

                    var lambda = Expression.Lambda<Func<T, bool>>(expre, target);
                    select = select.LeftJoin(lambda);
                }
            }
            return select;
        }
    }
}
