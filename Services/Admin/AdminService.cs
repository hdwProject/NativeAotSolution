using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Admin;
using Entity.User;
using Repositories.Admin;

namespace Services.Admin
{    
    public class AdminService(AdminInfoRepository adminInfoRepository) : ISingletonDependency
    {

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public async Task<AdminInfo> GetUserAsync() 
        { 
            var adminInfo = await adminInfoRepository.GetById(1212441110);

            var model = new AdminInfo
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
        public async Task<List<AdminInfo>> GetUserListAsync()
        {
            var userList = new List<AdminInfo>
            {
                new AdminInfo()
                {
                    Name = "测试-0",
                    Description= "Description0",
                    Email = "Email-0",
                    Phone ="12345564",
                    Address="湖北武汉",
                    City ="武汉"
                },
                new AdminInfo()
                {
                    Name = "测试-1",
                    Description= "Description1",
                    Email = "Email-1",
                    Phone ="12345564",
                    Address="湖北武汉",
                    City ="武汉"
                },
                new AdminInfo()
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
