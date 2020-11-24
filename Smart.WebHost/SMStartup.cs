using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Panda.DynamicWebApi;
using Smart.Application;
using Smart.Host.Converters;
using Smart.Host.Helpers;
using Smart.Infrastructure.Configs;
using Smart.Infrastructure.Configs.Helper;
using Smart.Infrastructure.Freesql;
using System;
using System.IO;

namespace Smart.Host
{
    /// <summary>
    /// host启动
    /// </summary>
    public class SMStartup
    {
        private readonly AppConfig _appConfig;
        private readonly DbConfig _dbConfig;
        private readonly JwtConfig _jwtConfig;
        private readonly string _corsName = "AllowCros";
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
        /// 注册跨域、
        /// </summary>
        /// <param name="services"></param>
        protected void AddSMServices(IServiceCollection services)
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
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "Crypto Smart",
                        Description = "Smart基础",
                        Contact = new OpenApiContact
                        {
                            Name = "ssd",
                            Email = "1292934053@qq.com",
                            Url = new Uri("http://cnblogs.com/shshshdy"),
                        },
                    });
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
                    var xmlFile = "Smart.Host.xml";
                    var xmlPath = Path.Combine(baseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath);

                    xmlFile = "Smart.Application.xml";
                    xmlPath = Path.Combine(baseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath);
                });
            }
            //动态webapi 
            services.AddDynamicWebApi();
        }
    }
}
