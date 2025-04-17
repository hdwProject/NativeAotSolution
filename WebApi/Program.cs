using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;
using System.Reflection;
using System.Text.Json;
using Serilog.Events;
using Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();

#region 配置Serilog

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", restrictedToMinimumLevel: LogEventLevel.Information, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
    .CreateLogger();
builder.Services.AddSerilog();
builder.Host.UseSerilog();

//builder.Services.AutofacConfigureContainer(builder, _plugins, typeof(Controller), typeof(IDenpendency), typeof(Program));



//var user = "werwer";
//SerilogHelper.Information("sdfsdf {@user}{@kk} asdasdasda", user,"kk sdfsdf");

#endregion

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<AuthData>();

//builder.Services.AutoRegisterServices(Assembly.GetExecutingAssembly(), ServiceLifetime.Singleton);

// 控制器自动发现 + 属性注入
//builder.Services.AddControllers().ConfigureApplicationPartManager(manager =>
//{
//    manager.FeatureProviders.Add(new AutoInjectControllerFeatureProvider());
//});

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

builder.Services.AddAuthorizationBuilder().AddPolicy(AuthData.Super, policy => policy.RequireRole(AuthData.Super));

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

var app = builder.Build();

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


//app.MapControllerRoute(
//        name: "default",
//        pattern: "api/{controller=Home}/{action=Index}/{id?}")
//    .RequireAuthorization();

app.MapControllerRoute(
        name: "default",
        pattern: "api",
        defaults: new { controller = "Home", action = "get" })
    .RequireAuthorization(AuthData.Super);

SerilogHelper.Information("Api站点启动成功");
app.Run();
