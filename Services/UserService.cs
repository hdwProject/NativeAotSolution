using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{    
    public class UserService : BaseService
    {
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public async Task<UserInfoR> GetUserAsync() 
        { 
            var model = new UserInfo
            {
                Name = "测试",
                Description = "Description",
                Email = "Email",
                Phone = "12345564",
                PhoneNumber = "1234567890",
                EmailAddress = "werwe@qq.com",
                Address = "湖北武汉",
                City = "武汉"

            };
            var r = new UserInfoR(DateTime.Now, "测试", "Description", "Email", "12345564", "12345564", "werwe@qq.com", "湖北武汉", "武汉");
          
            return await Task.FromResult(r);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<UserInfo>> GetUserListAsync()
        {
            var userList = new List<UserInfo>
            {
                new UserInfo()
                {
                    Name = "测试-0",
                    Description= "Description0",
                    Email = "Email-0",
                    Phone ="12345564",
                    PhoneNumber = "1234567890",
                    EmailAddress="werwe@qq.com",
                    Address="湖北武汉",
                    City ="武汉"
                },
                new UserInfo()
                {
                    Name = "测试-1",
                    Description= "Description1",
                    Email = "Email-1",
                    Phone ="12345564",
                    PhoneNumber = "1234567890",
                    EmailAddress="werwe@qq.com",
                    Address="湖北武汉",
                    City ="武汉"
                },
                new UserInfo()
                {
                    Name = "测试-2",
                    Description= "Description2",
                    Email = "Email-2",
                    Phone ="12345564",
                    PhoneNumber = "1234567890",
                    EmailAddress="werwe@qq.com",
                    Address="湖北武汉-2",
                    City ="武汉"
                },
            };
            return await Task.FromResult(userList);
        }
        public record UserInfoR( DateTime DateTime = new(), string Name = "", string Description = "", string Email = "", string Phone = "", string PhoneNumber = "", string EmailAddress = "", string Address = "", string City = "");
    }

    public class UserInfo
    { 
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;   
        public string Phone { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;         
        public string EmailAddress { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty; 
        public string City { get; set; } = string.Empty;    
        public DateTime DateTimeValue { get; set; } = DateTime.Now;
    }
   
    
}
