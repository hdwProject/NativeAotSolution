using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.AspNetCore.Authentication.Cookies;
using Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddJsonOptions(configure =>
{
    configure.JsonSerializerOptions.Converters.Add(new DateTimeToStringConverter());
    configure.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;//JavaScriptEncoder.Create(UnicodeRanges.All);// JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs);
    //configure.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;// 驼峰命名
    //configure.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    //configure.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull; // 序列化的时候忽略null值属性
    //configure.JsonSerializerOptions.PropertyNameCaseInsensitive = true; // 反序列化不区分大小写
    //configure.JsonSerializerOptions.WriteIndented = true; // 整齐打印
    //configure.JsonSerializerOptions.AllowTrailingCommas = false; // 不允许结尾有逗号的不标准json
}); 

builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<AuthData>();

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

    options.Cookie.Name = "AspNetCore.WebAppMvc";
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.Headers["Content-Type"] = "application/json";//这里必须设置，否则会出现乱码
        var msg = new
        {
            code = StatusCodes.Status401Unauthorized,
            message = "\"未授权，请使用登录接口授权\"",
        }.ToString();
        context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(msg));
        return Task.CompletedTask;
    };
    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    };
});
builder.Services.AddAuthorizationBuilder().AddPolicy(AuthData.Super, policy => policy.RequireRole(AuthData.Super));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();//一定要注意顺序，否则会导致401错误
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "api/{controller=Home}/{action=Index}/{id?}").RequireAuthorization();


app.Run();
