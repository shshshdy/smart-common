# 使用方法
## 目录结构
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
        return new string[] { Path.Combine(baseDirectory, "Cms.Application.xml") };
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
    
