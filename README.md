# 使用方法
每层使用对应nuget包
## 示例代码目录结构
```
project
│
└───webhost
│   │   Program.cs
│   │   Startup.cs
│   │   Startup.ConfigureContainer.cs
│   │   appsettings.json
│   └───Configs
│       │   appconfig.json
│       │   cacheconfig.json
│       │   dbconfig.json
│       │   jwtconfig.json
│       │   logconfig.json
│   
└───application
│   │   AutoMapperConfigs.cs
│   │   Permissions.cs
└───domain
│   │   EntitiyEnumAssembly.cs
└───Infrastructure
```
# 示例代码
## Program.cs 
```
 public class Program
  {
      public static void Main(string[] args)
      {
          CreateHostBuilder(args).Build().Run();
      }

      public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .UseServiceProviderFactory(new FreeSql.DynamicProxyServiceProviderFactory())
               .UseServiceProviderFactory(new AutofacServiceProviderFactory())

               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder.UseStartup<Startup>();
               });
  }
 ```
 # Startup.cs
 ```
public partial class Startup : SMStartup
{
    private readonly IHostEnvironment _env;

    protected override Type PermissionClass => typeof(Permissions);
    public Startup(IWebHostEnvironment env) : base(env)
    {
        _env = env;
    }


    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AutoMapperConfigs));

        AddConfigureServices(services);

    }

    protected override string[] AddSwaggerDocs()
    {
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        return new string[] { Path.Combine(baseDirectory, "demo.Application.xml") };
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {

        AddConfigure(app, env);

    }

    protected override void InitData(IApplicationBuilder applicationBuilder)
    {
        base.InitData(applicationBuilder);
        //TODO 热数据
    }
}
 ```
 ## Startup.ConfigureContainer.cs
 ```
/// <summary>
/// 模块注入
/// </summary>
/// <param name="builder"></param>
public void ConfigureContainer(ContainerBuilder builder)
{
    RegisterSmart(builder);

    Register(builder, "Cms.Application", "Service");
    Register(builder, "Cms.Domain");

}
 ```
 ## appconfig.json
 ```
 {
  "name": "demo system",
  //默认密码
  "defaultPwd": "123456",
  //Swagger文档
  "swagger": true,
  //api调用地址 多地址|分隔
  "urls": "http://localhost:8080|http://localhost:8081",
  "filePath": "files",
  "fileAuth": "ssd",
  "filePwd": "123",
  //日志
  "log": {
    //操作日志
    "operation": true
  }
}
 ```
 ## cacheconfig.josn
 ```
 {
  //缓存类型 Memory = 0,Redis = 1
  "type": 0,
  //Redis配置
  "redis": {
    "connectionString": "127.0.0.1:6379,password=,defaultDatabase=2"
  }
}
 ```
  ## dbconfig.json
 ```
{
  //生成数据
  "generateData": false,

  //同步结构
  "syncStructure": true,
  //同步数据
  "syncData": true,

  //建库
  "createDb": true,
  //SqlServer,PostgreSQL,Oracle,OdbcOracle,OdbcSqlServer,OdbcMySql,OdbcPostgreSQL,Odbc,OdbcDameng,MsAccess
  //建库连接字符串
  //MySql "Server=localhost; Port=3306; Database=mysql; Uid=root; Pwd=pwd; Charset=utf8mb4;"
  //SqlServer "Data Source=.;Integrated Security=True;Initial Catalog=master;Pooling=true;Min Pool Size=1"
  "createDbConnectionString": "Server=localhost; Port=3306; Database=mysql; Uid=root; Pwd=pwd; Charset=utf8mb4;",
  //建库脚本
  //MySql "CREATE DATABASE `cardb` CHARACTER SET 'utf8mb4' COLLATE 'utf8mb4_general_ci'"
  //SqlServer "CREATE DATABASE [cardb]"
  "createDbSql": "CREATE DATABASE `cardb` CHARACTER SET 'utf8mb4' COLLATE 'utf8mb4_general_ci'",

  //数据库类型 MySql = 0, SqlServer = 1, PostgreSQL = 2, Oracle = 3, Sqlite = 4, OdbcOracle = 5, OdbcSqlServer = 6, OdbcMySql = 7, OdbcPostgreSQL = 8, Odbc = 9, OdbcDameng = 10, MsAccess = 11
  "type": 0,
  //连接字符串
  //MySql "Server=localhost; Port=3306; Database=cardb; Uid=root; Pwd=pwd; Charset=utf8mb4;"
  //SqlServer "Data Source=.;Integrated Security=True;Initial Catalog=cardb;Pooling=true;Min Pool Size=1"
  //Sqlite "Data Source=|DataDirectory|\\cardb.db; Pooling=true;Min Pool Size=1"
  "connectionString": "Server=www.mir3.red; Port=3306; Database=cms; Uid=root; Pwd=123456; Charset=utf8mb4;"
}
 ```
 ## jwtconfig.json
 ```
{
  //发行者
  "issuer": "http://www.shshshdy.com",
  //订阅者
  "audience": "http://www.shshsdy.com",
  //秘钥
  "securityKey": "ertJKl#521*a@790asD&",
  //有效期(分钟)
  "expires": 120
}
 ```
  ## logconfig.json
 ```
{
  /*
  * https://nlog-project.org/config/
  * use
  private readonly ILogger<T> _logger;
  constructor(ILogger<T> logger)
  {
    _logger = logger;
  }
  _logger.LogDebug(1, "调试");

  或

  private readonly ILogger _logger;
  constructor()
  {
    _logger = LogManager.GetLogger("loggerName"); 
    或 
    _logger = LogManager.GetCurrentClassLogger();
  }
   _logger.Error("错误");
  */
  "nLog": {
    "extensions": {
      "NLog.Web.AspNetCore": {
        "assembly": "NLog.Web.AspNetCore"
      }
    },
    "targets": {
      //调试
      "debug": {
        "type": "File",
        "fileName": "logs\\debug-${shortdate}.log",
        "layout": "${longdate}|${event-properties:item=EventId_Id}|${uppercase:${level}}|${logger}|${message} ${exception:format=tostring}|url: ${aspnet-request-url}|action: ${aspnet-mvc-action}"
      },
      //警告
      "warn": {
        "type": "File",
        "fileName": "logs\\warn-${shortdate}.log",
        "layout": "${longdate}|${event-properties:item=EventId_Id}|${uppercase:${level}}|${logger}|${message} ${exception:format=tostring}|url: ${aspnet-request-url}|action: ${aspnet-mvc-action}"
      },
      //错误
      "error": {
        "type": "File",
        "fileName": "logs\\error-${shortdate}.log",
        "layout": "${longdate}|${event-properties:item=EventId_Id}|${uppercase:${level}}|${logger}|${message} ${exception:format=tostring}|url: ${aspnet-request-url}|action: ${aspnet-mvc-action}"
      }
    },
    "rules": [
      {
        "logger": "*",
        "minLevel": "Trace",
        "maxlevel": "Debug",
        "writeTo": "debug"
      },
      {
        "logger": "*",
        "minLevel": "Info",
        "maxlevel": "Warn",
        "writeTo": "warn"
      },
      {
        "logger": "*",
        "minLevel": "Error",
        "maxlevel": "Fatal",
        "writeTo": "error"
      },
      //跳过不重要的微软日志
      {
        "logger": "Microsoft.*",
        "maxLevel": "Info",
        "final": "true"
      }
    ]
  }
}
 ```
## AutoMapperConfigs.cs
```
 public class AutoMapperConfigs : Smart.Application.AutoMapperConfigs
 {
     public AutoMapperConfigs()
     {

     }
 }
```
## Permissions.cs
```
 public class Permissions : Smart.Application.Permissions
 {

 }
```
## EntitiyEnumAssembly.cs
```
public class EntitiyEnumAssembly : IEntitiyEnumAssembly
{
    public Assembly Assembly => typeof(EntitiyEnumAssembly).Assembly;
}
```
