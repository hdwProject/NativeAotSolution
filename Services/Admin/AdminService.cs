using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Entity.Admin;
using Entity.User;
using Repositories.Admin;
using SDK;
using SDK.Response.AdminInfo;
using SqlSugar;

namespace Services.Admin
{    
    public class AdminService(AdminInfoRepository adminInfoRepository) : ISingletonDependency
    {
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public async Task<AdminInfo> GetUserAsync(long id) 
        {
            return await adminInfoRepository.FindEntity(x=> x.Id == id);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public async Task<long> InsertAsync(AdminInfo model)
        {
            var adminInfo = await adminInfoRepository.Insert(model);
            return adminInfo.Id;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public async Task<BasePagerResponse<AdminInfo>> GetListAsync(GetListAdminInfoRequest request)
        {
            RefAsync<int> totalCount = 0;//REF和OUT不支持异步,想要真的异步这是最优解

            var query = adminInfoRepository.Db.Queryable<AdminInfo>();
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                query = query.Where(x => x.Name.Contains(request.Name));
            }
            if(!string.IsNullOrWhiteSpace(request.Email))
            {
                query = query.Where(x => x.Email.Contains(request.Email));
            }
            if(request.IsDelete.HasValue)
            {
                query = query.Where(x => x.IsDelete == request.IsDelete.Value);
            }

            if (request.StartDateTime.HasValue)
            {
                query = query.Where(x => x.CreateDateTime >= request.StartDateTime.Value);
            }
            if (request.EndDateTime.HasValue)
            {
                query = query.Where(x => x.CreateDateTime <= request.EndDateTime.Value);
            }
            var list = await query.OrderByPropertyName(request.OrderByField, request.OrderByType).ToOffsetPageAsync(request.PageIndex, request.PageSize, totalCount);

            return new BasePagerResponse<AdminInfo>{List = list, PageNo = request.PageIndex, PageSize = request.PageSize, TotalCount = totalCount};
        }
        
    }
}
