using AutoMapper;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Panda.DynamicWebApi;
using Smart.Application;
using Smart.Application.Systems.interfaces;
using Smart.Host.Converters;
using Smart.Host.Helpers;
using Smart.Infrastructure.Configs;
using Smart.Infrastructure.Configs.Helper;
using Smart.Infrastructure.Freesql;
using System;
using System.IO;
using System.Reflection;
using Smart.Host.Extensions;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Diagnostics;

namespace Smart.Host
{
    /// <summary>
    /// host启动
    /// </summary>
    public class SMStartup
    {
        /// <summary>
        /// app配置
        /// </summary>
        protected readonly AppConfig _appConfig;
        /// <summary>
        /// 数据库配置
        /// </summary>
        protected readonly DbConfig _dbConfig;
        /// <summary>
        /// jwt验证配置
        /// </summary>
        protected readonly JwtConfig _jwtConfig;

        private readonly string _corsName = "AllowCros";

        /// <summary>
        /// 权限定义,重写定义为 继承Permissions的类
        /// </summary>
        protected virtual Type PermissionClass
        {
            get {
                return typeof(Permissions);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="env"></param>
        public SMStartup(IWebHostEnvironment env)
        {
            _appConfig = ConfigHelper.Get<AppConfig>(env.EnvironmentName) ?? new AppConfig();
            _dbConfig = ConfigHelper.Get<DbConfig>(env.EnvironmentName) ?? new DbConfig();
            _jwtConfig = ConfigHelper.Get<JwtConfig>(env.EnvironmentName) ?? new JwtConfig();
        }

        /// <summary>
        /// 注册smart服务
        /// </summary>
        /// <param name="services"></param>
        protected void AddConfigureServices(IServiceCollection services)
        {
            //跨域设置
            services.AddCors(x =>
               x.AddPolicy(_corsName, builder => builder
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .WithOrigins(_appConfig.Urls.Split('|'))
               )
           );

            services.AddControllersWithViews()
                .AddNewtonsoftJson()
                .AddJsonOptions(options =>//JSON格式化
                {
                    options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                    options.JsonSerializerOptions.Converters.Add(new DateTimeNullableConverter());
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                });

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;//401
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;//403
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = JwtHelper.GetParameters(_jwtConfig);
            });

            services.AddFreesql(_appConfig, _dbConfig);

            services.AddAutoMapper(typeof(AutoMapperConfigs));

            if (_appConfig.Swagger)
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("Smart", new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "Smart",
                        Description = "Crypto Smart",
                        Contact = new OpenApiContact
                        {
                            Name = "ssd",
                            Email = "1292934053@qq.com",
                            Url = new Uri("http://cnblogs.com/shshshdy"),
                        },
                    });
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = $"{_appConfig.Name} Api", Version = "v1" });
                    //设置要展示的接口
                    c.DocInclusionPredicate((docName, apiDes) =>
                    {
                        if (!apiDes.TryGetMethodInfo(out MethodInfo method))
                            return false;
                        /*使用ApiExplorerSettingsAttribute里面的GroupName进行特性标识
                         * DeclaringType只能获取controller上的特性
                         * 我们这里是想以action的特性为主
                         * */
                        var version = method.DeclaringType.GetCustomAttributes(true).OfType<ApiExplorerSettingsAttribute>().Select(m => m.GroupName);
                        if (docName == "v1" && !version.Any())
                            return true;
                        //这里获取action的特性
                        var actionVersion = method.GetCustomAttributes(true).OfType<ApiExplorerSettingsAttribute>().Select(m => m.GroupName);
                        if (actionVersion.Any())
                            return actionVersion.Any(v => v == docName);
                        return version.Any(v => v == docName);
                    });
                    //jwt配置
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                    {
                        Description = "在下框中输入Jwt授权：Bearer Token",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        BearerFormat = "JWT",
                        Scheme = "Bearer"
                    });

                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] { }
                        }
                    });
                    // 加载程序集的xml描述文档
                    var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    var xmlFile = "Smart.WebHost.xml";
                    var xmlPath = Path.Combine(baseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath);

                    xmlFile = "Smart.Application.xml";
                    xmlPath = Path.Combine(baseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath);

                    var xmls = AddSwaggerDocs();
                    if (xmls.Length != 0)
                    {
                        foreach (var item in xmls)
                        {
                            c.IncludeXmlComments(item);
                        }
                    }
                });
            }

            //动态webapi 
            services.AddDynamicWebApi();
        }

        /// <summary>
        /// 添加app配置
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        protected void AddConfigure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.ApplicationServices.GetAutofacRoot()//获取容器对象

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            //跨域设置
            app.UseCors(_corsName);

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCustomExceptionMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

            });

            #region 热数据及权限数据
            var systemService = app.ApplicationServices.GetService(typeof(ISystemService)) as ISystemService;
            systemService.CreatAllPermissions(PermissionClass);

            if (!PermissionClass.IsSubclassOf(typeof(Permissions)))
            {
                systemService.CreatAllPermissions(typeof(Permissions));
            }
            InitData(app);
            #endregion

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/Smart/swagger.json", "SmartApi");
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api");
                // 访问Swagger的路由后缀
                c.RoutePrefix = "swagger";
            });
            //IdentityModelEventSource.ShowPII = true;
        }
        /// <summary>
        /// 添加swagger文档注释
        /// </summary>
        protected virtual string[] AddSwaggerDocs()
        {
            var arr = new string[0];
            return arr;
        }
        /// <summary>
        /// 初始化数据
        /// 可重写加入热数据
        /// </summary>
        /// <param name="applicationBuilder"></param>
        protected virtual void InitData(IApplicationBuilder applicationBuilder)
        {
            
        }

        /// <summary>
        /// 注入Smart
        /// </summary>
        protected void RegisterSmart(ContainerBuilder builder)
        {
            builder.Register(c => _jwtConfig).SingleInstance();
            builder.Register(c => _appConfig).SingleInstance();

            //service
            Register(builder, "Smart.Application", "Service");
            //manager
            Register(builder, "Smart.Domain");
        }

        /// <summary>
        /// 程序集注入
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="nameSpace">程序集名称</param>
        /// <param name="endPart">结尾名</param>
        protected void Register(ContainerBuilder builder, string nameSpace, string endPart)
        {
            Assembly manager = Assembly.Load(nameSpace);
            builder.RegisterAssemblyTypes(manager)
            .Where(t => t.Name.EndsWith(endPart))
            .AsImplementedInterfaces()
            .InstancePerDependency()
            .EnableInterfaceInterceptors();
        }
        /// <summary>
        /// 程序集注入
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="nameSpace">程序集名称</param>
        protected void Register(ContainerBuilder builder, string nameSpace)
        {
            Assembly manager = Assembly.Load(nameSpace);
            builder.RegisterAssemblyTypes(manager)
            .AsImplementedInterfaces()
            .InstancePerDependency()
            .EnableInterfaceInterceptors();
        }
    }
}
