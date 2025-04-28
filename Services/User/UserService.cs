using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.User;

namespace Services.User
{    
    public class UserService : ISingletonDependency
    {
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public async Task<UserInfo> GetUserAsync() 
        { 
            var model = new UserInfo
            {
                Name = "测试",
                Description = "Description",
                Email = "Email",
                Phone = "12345564",
                Address = "湖北武汉",
                City = "武汉"

            };
            return await Task.FromResult(model);
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
                    Address="湖北武汉",
                    City ="武汉"
                },
                new UserInfo()
                {
                    Name = "测试-1",
                    Description= "Description1",
                    Email = "Email-1",
                    Phone ="12345564",
                    Address="湖北武汉",
                    City ="武汉"
                },
                new UserInfo()
                {
                    Name = "测试-2",
                    Description= "Description2",
                    Email = "Email-2",
                    Phone ="12345564",
                    Address="湖北武汉-2",
                    City ="武汉"
                },
            };
            return await Task.FromResult(userList);
        }
        
    }
}
