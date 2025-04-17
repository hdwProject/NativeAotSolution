using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Diagnostics;
using System.Security.Claims;
using Utility;
using WebAppMvc.Models;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebAppMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public static readonly UserService UserService = new UserService();
        
        public async Task<IActionResult> Get()
        {
            var adminId = AuthData.AdminIdField;
            var userInfo = await UserService.GetUserAsync();
            var userInfoList = await UserService.GetUserListAsync();
            var randomUserInfo = userInfoList[Random.Shared.Next(0, userInfoList.Count)];
            return Ok(new { adminId, userInfo, randomUserInfo });
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [AllowAnonymous]
        public async Task<IActionResult> Login(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return Content("请输入账号");
            }

            if (string.IsNullOrEmpty(password))
            {
                return Content("请输入密码");
            }
            var userInfo = await UserService.GetUserAsync();
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(new ClaimsIdentity(
                    [
                        new Claim(AuthData.AdminIdField, Guid.NewGuid().ToString("n")),
                        new Claim(AuthData.AdminNameField, "e34dfggde25441"),
                        new Claim(userInfo.EmailAddress, "1234564478454sfsersdfsff"),
                        new Claim("Role", "Admin"),
                        new Claim(userInfo.Email, "Admin@qq.com"),
                        new Claim("ImageTop", "Admin.jpg"),
                        new Claim("Gold", "45454"),
                        new Claim("LogTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                        new Claim(ClaimTypes.Role, "new List<string>{\"sfsdf\",\"sdfsdf DateTime.Now:yyyy-MM-dd HH:mm:ss"),
                    ],
                    CookieAuthenticationDefaults.AuthenticationScheme)), new AuthenticationProperties { IsPersistent = true });

            return Ok(new
            {
                UserId = "merchant.Id",
                UserName = "merchant.Account",
                datatime = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}",
                Roles = new[] { "AuthData.Merchant" }
            });
        }

       
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return NoContent();
        }
        
        //[Authorize]
        public async Task<IActionResult> TestGet()
        {
            var userName = HttpContext.User.Claims.FirstOrDefault(x => x.Type == AuthData.AdminNameField)?.Value ?? string.Empty;
            var role = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value ?? string.Empty;
            var s =  GeTask().Result.Select(x => new { x.userName, x.password, x.age }).ToList();
            var tt = s.SerializeString();

            var model = tt.DeserializeObject<List<object>>();

            var s1 = SecurityHelper.Md5Encrypt(tt);

            return Ok(new {data = tt, role, s1 }.SerializeString());//如果使用Ok返回json格式，会出现反斜杠“\”, 
            //return Content(JsonHelper.Serialize(new { data = s, role }));//Content 返回json格式不会出现反斜杠“\”
        }

        private static async Task<List<(string userName, string password, int age)>> GeTask()
        {
            var list = new List<(string userName,string password, int age)>
            {
                ("123", "123456", 11),
                ("456", "123456", 12),
                ("789", "123456", 13),
            };
            return await Task.FromResult(list);
        }
    }
}
