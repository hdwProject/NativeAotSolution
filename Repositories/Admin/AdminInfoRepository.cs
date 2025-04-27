using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Admin;
using SqlSugar;

namespace Repositories.Admin
{
    public class AdminInfoRepository(ISqlSugarClient scope) : BaseRepository<AdminInfo>(scope)
    {
        /// <summary>
        /// 根据名称获取管理员信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<AdminInfo> GetByName(string name)
        {
            return await Db.Queryable<AdminInfo>().Where(x => x.Name == name).FirstAsync();
        }
        /// <summary>
        /// 根据Id获取管理员信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<AdminInfo> GetById(long id)
        {
            return await Db.Queryable<AdminInfo>().Where(x => x.Id == id).FirstAsync();
        }

    }


}
