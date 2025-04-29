using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;
using System.Reflection;
using Serilog.Events;
using Utility;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Services;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Services.User;
using WebApi;
using Autofac.Core;
using SqlSugar;
using Utility.Globals;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);



#region 配置Serilog

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", restrictedToMinimumLevel: LogEventLevel.Information, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true, 
        outputTemplate: "时间：{Timestamp:yyyy-MM-dd HH:mm:ss.fff} => 级别：{Level:u3}{NewLine}日志信息：{Message}{NewLine}" +
                        "-------------------------------------------------------------------------------------------------------------------------------------------------------------------{NewLine}")
    .CreateLogger();
builder.Services.AddSerilog();
builder.Host.UseSerilog();

//builder.Services.AutofacConfigureContainer(builder, _plugins, typeof(Controller), typeof(IDenpendency), typeof(Program));



//var user = "werwer";
//SerilogHelper.Information("sdfsdf {@user}{@kk} asdasdasda", user,"kk sdfsdf");

#endregion

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
    });
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(120);
    
    options.Cookie.Name = ".AspNetCore.WebApi";
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.Redirect("/api/Home/get");
        return Task.CompletedTask;
    };
    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    };
});

builder.Services.AddHttpClient();

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
//    options.AddPolicy("RequireCustomPolicy", policy => policy.RequireAssertion(context =>
//    {
//        var user = context.User;
//        // 自定义授权逻辑，例如检查用户属性等。 
//        return user.HasClaim(c => c.Type == "customClaim" && c.Value == "true");
//    }));
//});

builder.Services.AddControllersWithViews().AddJsonOptions(option =>
{
    //添加时间格式化，解决接口使用OK方法的时候，时间格式问题
    option.JsonSerializerOptions.Converters.Add(new DateTimeToStringConverter());
});

// Add services to the container.这里必须要加上AddControllersAsServices方法，否则属性注入会失败
//builder.Services.AddControllers();

GlobalContext.SystemConfig = builder.Configuration.GetSection("SystemConfig").Get<SystemConfig>() ?? new SystemConfig();
GlobalContext.Configuration = builder.Configuration;
GlobalContext.Services = builder.Services;

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<AuthData>();
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

//builder.Services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());
// 将Controller加入到Services中，这样写上面的代码就可以省略了,
builder.Services.AddControllers();//.AddControllersAsServices();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(options =>
{
    options.RegisterModule(new RegisterAutofacModule());

    //options.AutofacConfigureContainer(["Services"], typeof(Controller), typeof(IDependency), typeof(Program));
    //MyServiceCollectionExtensions.AutofacConfigureContainer(options, ["Services"], typeof(Controller), typeof(IDependency), typeof(Program));
    //options.AutofacConfigureContainer(["Services"], typeof(ControllerBase), typeof(IDependency), typeof(Program));
});



//初始化雪花算法ID
YitterHelper.InitIdHelper();

var app = builder.Build();

AutofacContainerManager.InitContainer(app.Services.GetAutofacRoot()); 

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
}

app.UseRouting();
app.UseAuthentication();    // 如果需要身份验证
app.UseAuthorization();      // 必须在 UseRouting 之后

app.UseHttpsRedirection();

//app.MapControllers();


app.MapControllerRoute(name: "default", pattern: "api/{controller=Home}/{action=Index}/{id?}");

GlobalContext.LogWhenStart(app.Environment);
//SerilogHelper.Information("Api站点启动成功");
app.Run();
