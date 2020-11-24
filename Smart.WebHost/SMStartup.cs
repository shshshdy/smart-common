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

namespace Smart.Host
{
    /// <summary>
    /// host启动
    /// </summary>
    public class SMStartup
    {
        protected readonly AppConfig _appConfig;
        protected readonly DbConfig _dbConfig;
        protected readonly JwtConfig _jwtConfig;
        protected readonly string _corsName = "AllowCros";
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
                        Version = "Smart",
                        Title = "Crypto Smart",
                        Description = "Smart Base",
                        Contact = new OpenApiContact
                        {
                            Name = "ssd",
                            Email = "1292934053@qq.com",
                            Url = new Uri("http://cnblogs.com/shshshdy"),
                        },
                    });
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "api说明", Version = "v1" });
                    // TODO:一定要返回true！
                    c.DocInclusionPredicate((doc, des) => true);
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
                    Console.WriteLine(xmlPath);
                    c.IncludeXmlComments(xmlPath);

                    xmlFile = "Smart.Application.xml";
                    xmlPath = Path.Combine(baseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath);

                    var xmls = AddSwaggerDocs();
                    if (xmls.Length != 0)
                    {
                        foreach(var item in xmls)
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

            InitData(app);

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/Smart/swagger.json", "Crypto Smart");
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Crypto 云博");
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
            var arr= new string[0];
            return arr;
        }
        /// <summary>
        /// 初始化数据
        /// 可重写加入热数据
        /// </summary>
        /// <param name="applicationBuilder"></param>
        protected virtual void InitData(IApplicationBuilder applicationBuilder)
        {
            var systemService = applicationBuilder.ApplicationServices.GetService(typeof(ISystemService)) as ISystemService;

            systemService.CreatAllPermissions();
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
        protected void Register(ContainerBuilder builder,string nameSpace, string endPart)
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
