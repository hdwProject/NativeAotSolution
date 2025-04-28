using System.Security.Claims;
using Autofac.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Admin;
using Services.User;
using Utility;
using Utility.CustomerAttribute;
using Utility.Globals;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class HomeController(AdminService service, IHttpClientFactory httpClientFactory) : BaseController
    {
        //[AutowiredDependencyProperty]
        //public AdminService AdminService { get; set; }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var client = httpClientFactory.CreateClient();
            var s = await service.GetUserAsync();
            var c = AutofacContainerManager.Resolve<AdminService>();
            var adminId = AuthData.AdminIdField;
            var userInfo = await service.GetUserAsync();
            return Ok(userInfo);
        }

        [HttpGet("Login")]
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
            var userInfo = await service.GetUserAsync();
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(new ClaimsIdentity(
                    [
                        new Claim(AuthData.AdminIdField, Guid.NewGuid().ToString("n")),
                        new Claim(AuthData.AdminNameField, "e34dfggde25441"),
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

        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return NoContent();
        }

        [HttpGet("TestGet")]
        //[Authorize]
        public async Task<IActionResult> TestGet()
        {
            var userName = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserName")?.Value ?? string.Empty;
            var role = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value ?? string.Empty;
            var s = await GeTask();
            var a = s.address;
            return Ok(new { data = s });
        }

        private async Task<(string userName, string passWord, string address)> GeTask()
        {

            return await Task.FromResult(("12", "122112", "12werwer"));
        }
    }
}
